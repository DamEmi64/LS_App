package com.lsapp.client;

import android.Manifest;
import android.content.Intent;
import android.net.Uri;
import android.media.MediaCodec;
import android.media.MediaExtractor;
import android.media.MediaFormat;
import android.os.Handler;
import android.os.Looper;
import com.k2fsa.sherpa.onnx.FeatureConfig;
import com.k2fsa.sherpa.onnx.OfflineModelConfig;
import com.k2fsa.sherpa.onnx.OfflineRecognizer;
import com.k2fsa.sherpa.onnx.OfflineRecognizerConfig;
import com.k2fsa.sherpa.onnx.OfflineStream;
import com.k2fsa.sherpa.onnx.OfflineWhisperModelConfig;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.nio.ByteBuffer;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.Locale;
import java.util.UUID;
import androidx.core.content.FileProvider;
import java.io.File;

import com.getcapacitor.Plugin;
import com.getcapacitor.PluginCall;
import com.getcapacitor.PluginMethod;
import com.getcapacitor.JSObject;
import com.getcapacitor.annotation.CapacitorPlugin;
import com.getcapacitor.annotation.Permission;
import com.getcapacitor.annotation.PermissionCallback;

@CapacitorPlugin(
    name = "MeetingRecorder",
    permissions = @Permission(alias = "microphone", strings = { Manifest.permission.RECORD_AUDIO })
)
public class MeetingRecorderPlugin extends Plugin {
    private static final String MODEL_REVISION = "18e7b4cce9526312a66981ec3af7b9446b70db66";
    private static final String MODEL_REPOSITORY = "https://huggingface.co/csukuangfj/sherpa-onnx-whisper-tiny/resolve/" + MODEL_REVISION + "/";
    private static final long[] MODEL_FILE_WEIGHTS = { 13L, 90L, 1L };
    private static final long MODEL_TOTAL_WEIGHT = 104L;
    private final Handler mainHandler = new Handler(Looper.getMainLooper());
    private PluginCall pendingStartCall;

    @PluginMethod
    public void startRecording(PluginCall call) {
        if (MeetingRecordingService.isRecording()) {
            call.reject("A meeting recording is already active");
            return;
        }

        if (getPermissionState("microphone") != com.getcapacitor.PermissionState.GRANTED) {
            pendingStartCall = call;
            requestPermissionForAlias("microphone", call, "microphonePermissionResult");
            return;
        }

        startService(call);
    }

    @PermissionCallback
    private void microphonePermissionResult(PluginCall call) {
        pendingStartCall = null;
        if (getPermissionState("microphone") != com.getcapacitor.PermissionState.GRANTED) {
            call.reject("Microphone permission is required to record a meeting");
            return;
        }
        startService(call);
    }

    private void startService(PluginCall call) {
        String meetingId = call.getString("meetingId");
        if (meetingId == null || meetingId.trim().isEmpty()) {
            call.reject("meetingId is required");
            return;
        }

        Intent intent = new Intent(getContext(), MeetingRecordingService.class);
        intent.setAction(MeetingRecordingService.ACTION_START);
        intent.putExtra(MeetingRecordingService.EXTRA_MEETING_ID, meetingId);
        try {
            getContext().startForegroundService(intent);
            new android.os.Handler(getActivity().getMainLooper()).postDelayed(() -> {
                if (MeetingRecordingService.isRecording()) call.resolve();
                else call.reject("Android could not start the meeting recording");
            }, 350);
        } catch (Exception error) {
            call.reject("Unable to start the meeting recording", error);
        }
    }

    @PluginMethod
    public void stopRecording(PluginCall call) {
        try {
            String path = MeetingRecordingService.stopRecording(getContext());
            if (path == null) {
                call.reject("There is no active meeting recording");
                return;
            }
            JSObject result = new JSObject();
            result.put("path", path);
            call.resolve(result);
        } catch (Exception error) {
            call.reject("Unable to stop the meeting recording", error);
        }
    }

    @PluginMethod
    public void getRecordingState(PluginCall call) {
        JSObject result = new JSObject();
        result.put("recording", MeetingRecordingService.isRecording());
        result.put("path", MeetingRecordingService.getActivePath());
        result.put("meetingId", MeetingRecordingService.getActiveMeetingId());
        call.resolve(result);
    }

    @PluginMethod
    public void openRecording(PluginCall call) {
        String path = call.getString("path");
        if (path == null) {
            call.reject("Recording path is required");
            return;
        }
        File recording = new File(path);
        if (!recording.isFile()) {
            call.reject("Recording file was not found on this device");
            return;
        }
        try {
            Uri uri = FileProvider.getUriForFile(getContext(), getContext().getPackageName() + ".fileprovider", recording);
            Intent intent = new Intent(Intent.ACTION_VIEW);
            intent.setDataAndType(uri, "audio/mp4");
            intent.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION | Intent.FLAG_ACTIVITY_NEW_TASK);
            getContext().startActivity(intent);
            call.resolve();
        } catch (Exception error) {
            call.reject("No audio player is available for this recording", error);
        }
    }

    @PluginMethod
    public void transcribeRecording(PluginCall call) {
        String path = call.getString("path");
        if (path == null || !new File(path).isFile()) {
            call.reject("Recording file was not found on this device");
            return;
        }
        new Thread(() -> {
            try {
                File modelDirectory = ensureWhisperModel();
                reportProgress("transcribing", 0);
                String result = transcribeLocally(new File(path), modelDirectory);
                if (result.trim().isEmpty()) throw new IllegalStateException("No speech was recognized in this recording.");
                JSObject response = new JSObject();
                response.put("transcript", result.trim());
                mainHandler.post(() -> call.resolve(response));
            } catch (Exception error) {
                String message = error.getMessage() == null ? "On-device transcription failed." : error.getMessage();
                mainHandler.post(() -> call.reject(message, error));
            }
        }, "meeting-whisper-transcription").start();
    }

    private File ensureWhisperModel() throws Exception {
        File directory = new File(getContext().getFilesDir(), "meeting-models/whisper-tiny-int8");
        if (!directory.exists() && !directory.mkdirs()) throw new IllegalStateException("Unable to create private model storage.");
        String[] names = { "tiny-encoder.int8.onnx", "tiny-decoder.int8.onnx", "tiny-tokens.txt" };
        long[] minimumSizes = { 10L * 1024 * 1024, 80L * 1024 * 1024, 100L * 1024 };
        boolean ready = true;
        for (int i = 0; i < names.length; i++) {
            File file = new File(directory, names[i]);
            if (!file.isFile() || file.length() < minimumSizes[i]) { ready = false; break; }
        }
        if (ready) return directory;

        long completedWeight = 0;
        for (int i = 0; i < names.length; i++) {
            File destination = new File(directory, names[i]);
            if (destination.isFile() && destination.length() >= minimumSizes[i]) {
                completedWeight += MODEL_FILE_WEIGHTS[i];
                continue;
            }
            File partial = new File(directory, names[i] + ".part");
            downloadModelFile(MODEL_REPOSITORY + names[i] + "?download=true", partial, i, completedWeight);
            if (!partial.renameTo(destination)) {
                partial.delete();
                throw new IllegalStateException("Unable to save the downloaded transcription model.");
            }
            completedWeight += MODEL_FILE_WEIGHTS[i];
        }
        reportProgress("download", 100);
        return directory;
    }

    private void downloadModelFile(String address, File target, int fileIndex, long completedWeight) throws Exception {
        HttpURLConnection connection = (HttpURLConnection) new URL(address).openConnection();
        connection.setConnectTimeout(20000);
        connection.setReadTimeout(60000);
        connection.setRequestProperty("User-Agent", "LS-App-Android/1.1");
        try {
            int response = connection.getResponseCode();
            if (response != HttpURLConnection.HTTP_OK) throw new IllegalStateException("Model download failed (HTTP " + response + "). Check your internet connection and retry.");
            long fileLength = connection.getContentLengthLong();
            try (java.io.InputStream input = connection.getInputStream(); FileOutputStream output = new FileOutputStream(target, false)) {
                byte[] buffer = new byte[64 * 1024];
                long downloaded = 0;
                int lastReportedProgress = -1;
                int count;
                while ((count = input.read(buffer)) != -1) {
                    output.write(buffer, 0, count);
                    downloaded += count;
                    if (fileLength > 0) {
                        double progress = (completedWeight + MODEL_FILE_WEIGHTS[fileIndex] * (downloaded / (double) fileLength)) / MODEL_TOTAL_WEIGHT;
                        int currentProgress = Math.min(99, (int) Math.round(progress * 100));
                        if (currentProgress != lastReportedProgress) {
                            lastReportedProgress = currentProgress;
                            reportProgress("download", currentProgress);
                        }
                    }
                }
                output.getFD().sync();
                if (fileLength > 0 && downloaded != fileLength) throw new IllegalStateException("The transcription model download was incomplete. Please retry.");
            }
        } finally {
            connection.disconnect();
        }
    }

    private void reportProgress(String phase, int progress) {
        JSObject event = new JSObject();
        event.put("phase", phase);
        event.put("progress", progress);
        mainHandler.post(() -> notifyListeners("transcriptionProgress", event));
    }

    private String transcribeLocally(File recording, File modelDirectory) throws Exception {
        File pcmFile = new File(getContext().getCacheDir(), "meeting-transcription-" + UUID.randomUUID() + ".pcm");
        OfflineRecognizer recognizer = null;
        try {
            decodeRecordingToPcm(recording, pcmFile);
            OfflineModelConfig model = new OfflineModelConfig();
            OfflineWhisperModelConfig whisper = new OfflineWhisperModelConfig();
            whisper.setEncoder(new File(modelDirectory, "tiny-encoder.int8.onnx").getAbsolutePath());
            whisper.setDecoder(new File(modelDirectory, "tiny-decoder.int8.onnx").getAbsolutePath());
            whisper.setLanguage("pl");
            whisper.setTask("transcribe");
            whisper.setTailPaddings(300);
            model.setWhisper(whisper);
            model.setTokens(new File(modelDirectory, "tiny-tokens.txt").getAbsolutePath());
            model.setProvider("cpu");
            model.setNumThreads(Math.max(2, Math.min(4, Runtime.getRuntime().availableProcessors() - 1)));
            OfflineRecognizerConfig config = new OfflineRecognizerConfig();
            FeatureConfig features = new FeatureConfig();
            features.setSampleRate(16000);
            features.setFeatureDim(80);
            config.setFeatConfig(features);
            config.setModelConfig(model);
            config.setDecodingMethod("greedy_search");
            recognizer = new OfflineRecognizer(null, config);

            long pcmLength = pcmFile.length();
            if (pcmLength < 2) return "";
            final int sampleRate = 16000;
            final int chunkSeconds = 30;
            final int overlapSeconds = 1;
            final long bytesPerSecond = sampleRate * 2L;
            final long chunkBytes = chunkSeconds * bytesPerSecond;
            final long strideBytes = (chunkSeconds - overlapSeconds) * bytesPerSecond;
            StringBuilder transcript = new StringBuilder();
            try (java.io.RandomAccessFile input = new java.io.RandomAccessFile(pcmFile, "r")) {
                long start = 0;
                while (start < pcmLength) {
                    input.seek(start);
                    int wanted = (int) Math.min(chunkBytes, pcmLength - start);
                    byte[] pcm = new byte[wanted];
                    int read = input.read(pcm);
                    if (read <= 0) break;
                    if (read < pcm.length) pcm = java.util.Arrays.copyOf(pcm, read);
                    float[] samples = new float[pcm.length / 2];
                    for (int i = 0; i < samples.length; i++) {
                        short sample = (short) ((pcm[i * 2 + 1] << 8) | (pcm[i * 2] & 0xff));
                        samples[i] = sample / 32768.0f;
                    }
                    OfflineStream stream = recognizer.createStream();
                    try {
                        stream.acceptWaveform(samples, sampleRate);
                        recognizer.decode(stream);
                        String part = recognizer.getResult(stream).getText().trim();
                        appendTranscriptPart(transcript, part);
                    } finally {
                        stream.release();
                    }
                    long next = start + strideBytes;
                    if (next >= pcmLength) break;
                    start = next;
                    reportProgress("transcribing", (int) Math.min(99, start * 100L / pcmLength));
                }
            }
            reportProgress("transcribing", 100);
            return transcript.toString().trim();
        } finally {
            pcmFile.delete();
            if (recognizer != null) recognizer.release();
        }
    }

    private void appendTranscriptPart(StringBuilder transcript, String part) {
        if (part == null || part.isEmpty()) return;
        if (transcript.length() == 0) { transcript.append(part); return; }
        String[] previous = transcript.toString().trim().split("\\s+");
        String[] next = part.split("\\s+");
        int overlap = 0;
        for (int candidate = Math.min(12, Math.min(previous.length, next.length)); candidate > 0; candidate--) {
            boolean matches = true;
            for (int i = 0; i < candidate; i++) {
                String left = previous[previous.length - candidate + i].toLowerCase(Locale.ROOT).replaceAll("[^\\p{L}\\p{N}]", "");
                String right = next[i].toLowerCase(Locale.ROOT).replaceAll("[^\\p{L}\\p{N}]", "");
                if (!left.equals(right)) { matches = false; break; }
            }
            if (matches) { overlap = candidate; break; }
        }
        for (int i = overlap; i < next.length; i++) transcript.append(' ').append(next[i]);
    }

    private void decodeRecordingToPcm(File recording, File pcmFile) throws Exception {
        MediaExtractor extractor = new MediaExtractor();
        MediaCodec decoder = null;
        try (FileOutputStream output = new FileOutputStream(pcmFile, false)) {
            extractor.setDataSource(recording.getAbsolutePath());
            int audioTrack = -1;
            MediaFormat format = null;
            for (int i = 0; i < extractor.getTrackCount(); i++) {
                MediaFormat candidate = extractor.getTrackFormat(i);
                String mime = candidate.getString(MediaFormat.KEY_MIME);
                if (mime != null && mime.startsWith("audio/")) { audioTrack = i; format = candidate; break; }
            }
            if (audioTrack < 0 || format == null) throw new IllegalArgumentException("No audio stream found in recording");
            extractor.selectTrack(audioTrack);
            String mime = format.getString(MediaFormat.KEY_MIME);
            decoder = MediaCodec.createDecoderByType(mime);
            decoder.configure(format, null, null, 0);
            decoder.start();
            int sourceRate = format.containsKey(MediaFormat.KEY_SAMPLE_RATE) ? format.getInteger(MediaFormat.KEY_SAMPLE_RATE) : 44100;
            int sourceChannels = format.containsKey(MediaFormat.KEY_CHANNEL_COUNT) ? format.getInteger(MediaFormat.KEY_CHANNEL_COUNT) : 1;
            MediaCodec.BufferInfo info = new MediaCodec.BufferInfo();
            boolean inputDone = false, outputDone = false;
            while (!outputDone) {
                if (!inputDone) {
                    int inputIndex = decoder.dequeueInputBuffer(10000);
                    if (inputIndex >= 0) {
                        ByteBuffer input = decoder.getInputBuffer(inputIndex);
                        int size = extractor.readSampleData(input, 0);
                        if (size < 0) {
                            decoder.queueInputBuffer(inputIndex, 0, 0, 0, MediaCodec.BUFFER_FLAG_END_OF_STREAM);
                            inputDone = true;
                        } else {
                            decoder.queueInputBuffer(inputIndex, 0, size, extractor.getSampleTime(), 0);
                            extractor.advance();
                        }
                    }
                }
                int outputIndex = decoder.dequeueOutputBuffer(info, 10000);
                if (outputIndex == MediaCodec.INFO_OUTPUT_FORMAT_CHANGED) {
                    MediaFormat decoded = decoder.getOutputFormat();
                    if (decoded.containsKey(MediaFormat.KEY_SAMPLE_RATE)) sourceRate = decoded.getInteger(MediaFormat.KEY_SAMPLE_RATE);
                    if (decoded.containsKey(MediaFormat.KEY_CHANNEL_COUNT)) sourceChannels = decoded.getInteger(MediaFormat.KEY_CHANNEL_COUNT);
                }
                if (outputIndex >= 0) {
                    ByteBuffer pcm = decoder.getOutputBuffer(outputIndex);
                    if (pcm != null && info.size > 0) {
                        pcm.position(info.offset); pcm.limit(info.offset + info.size);
                        byte[] bytes = new byte[info.size]; pcm.get(bytes);
                        byte[] converted = convertPcmToMono16k(bytes, sourceRate, sourceChannels);
                        output.write(converted);
                    }
                    outputDone = (info.flags & MediaCodec.BUFFER_FLAG_END_OF_STREAM) != 0;
                    decoder.releaseOutputBuffer(outputIndex, false);
                }
            }
        } catch (Exception error) {
            throw new IllegalStateException("Unable to prepare the recording for local transcription: " + error.getMessage(), error);
        } finally {
            try { extractor.release(); } catch (Exception ignored) {}
            if (decoder != null) { try { decoder.stop(); } catch (Exception ignored) {} decoder.release(); }
        }
    }

    private byte[] convertPcmToMono16k(byte[] input, int sampleRate, int channels) {
        int inputSamples = input.length / (2 * Math.max(1, channels));
        int outputSamples = (int) (inputSamples * 16000.0 / Math.max(1, sampleRate));
        byte[] output = new byte[outputSamples * 2];
        for (int i = 0; i < outputSamples; i++) {
            int source = Math.min(inputSamples - 1, (int) (i * Math.max(1, sampleRate) / 16000.0));
            int total = 0;
            for (int channel = 0; channel < Math.max(1, channels); channel++) {
                int offset = (source * channels + channel) * 2;
                total += (short) ((input[offset + 1] << 8) | (input[offset] & 0xff));
            }
            short mono = (short) (total / Math.max(1, channels));
            output[i * 2] = (byte) (mono & 0xff);
            output[i * 2 + 1] = (byte) ((mono >> 8) & 0xff);
        }
        return output;
    }

}
