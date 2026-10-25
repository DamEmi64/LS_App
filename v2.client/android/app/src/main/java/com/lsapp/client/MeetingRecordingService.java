package com.lsapp.client;

import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.content.pm.ServiceInfo;
import android.media.MediaRecorder;
import android.os.Build;
import android.os.IBinder;

import androidx.core.app.NotificationCompat;

import java.io.File;
import java.io.IOException;
import java.util.UUID;

public class MeetingRecordingService extends Service {
    static final String ACTION_START = "com.lsapp.client.meeting.START";
    static final String EXTRA_MEETING_ID = "meetingId";
    private static final String CHANNEL_ID = "meeting_recording";
    private static final int NOTIFICATION_ID = 7191;
    private static MediaRecorder recorder;
    private static String activePath;
    private static String activeMeetingId;

    @Override
    public void onCreate() {
        super.onCreate();
        createNotificationChannel();
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        if (intent == null || !ACTION_START.equals(intent.getAction())) {
            stopSelf(startId);
            return START_NOT_STICKY;
        }

        String meetingId = intent.getStringExtra(EXTRA_MEETING_ID);
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.Q) {
            startForeground(NOTIFICATION_ID, buildNotification(), ServiceInfo.FOREGROUND_SERVICE_TYPE_MICROPHONE);
        } else {
            startForeground(NOTIFICATION_ID, buildNotification());
        }
        try {
            startCapture(meetingId);
        } catch (Exception error) {
            stopForeground(STOP_FOREGROUND_REMOVE);
            stopSelf(startId);
        }
        return START_NOT_STICKY;
    }

    private synchronized void startCapture(String meetingId) throws IOException {
        if (recorder != null) return;

        File directory = new File(getFilesDir(), "meeting-recordings");
        if (!directory.exists() && !directory.mkdirs()) {
            throw new IOException("Unable to create private meeting recording directory");
        }
        String safeId = meetingId.replaceAll("[^A-Za-z0-9_-]", "_");
        File output = new File(directory, safeId + "_" + UUID.randomUUID() + ".m4a");

        MediaRecorder next = Build.VERSION.SDK_INT >= Build.VERSION_CODES.S
            ? new MediaRecorder(this)
            : new MediaRecorder();
        next.setAudioSource(MediaRecorder.AudioSource.MIC);
        next.setOutputFormat(MediaRecorder.OutputFormat.MPEG_4);
        next.setAudioEncoder(MediaRecorder.AudioEncoder.AAC);
        next.setAudioSamplingRate(44100);
        next.setAudioEncodingBitRate(96000);
        next.setOutputFile(output.getAbsolutePath());
        next.prepare();
        next.start();
        recorder = next;
        activePath = output.getAbsolutePath();
        activeMeetingId = meetingId;
    }

    static synchronized boolean isRecording() {
        return recorder != null;
    }

    static synchronized String getActivePath() {
        return activePath;
    }

    static synchronized String getActiveMeetingId() {
        return activeMeetingId;
    }

    static synchronized String stopRecording(Context context) {
        if (recorder == null) return null;

        String completedPath = activePath;
        try {
            recorder.stop();
        } catch (RuntimeException error) {
            new File(completedPath).delete();
            completedPath = null;
        } finally {
            recorder.reset();
            recorder.release();
            recorder = null;
            activePath = null;
            activeMeetingId = null;
        }

        context.stopService(new Intent(context, MeetingRecordingService.class));
        return completedPath;
    }

    private Notification buildNotification() {
        return new NotificationCompat.Builder(this, CHANNEL_ID)
            .setContentTitle("RPG meeting recording")
            .setContentText("Recording is saved only on this device")
            .setSmallIcon(android.R.drawable.ic_btn_speak_now)
            .setOngoing(true)
            .setCategory(NotificationCompat.CATEGORY_SERVICE)
            .build();
    }

    private void createNotificationChannel() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            NotificationChannel channel = new NotificationChannel(
                CHANNEL_ID, "Meeting recording", NotificationManager.IMPORTANCE_LOW);
            getSystemService(NotificationManager.class).createNotificationChannel(channel);
        }
    }

    @Override
    public void onDestroy() {
        if (recorder != null) stopRecording(this);
        super.onDestroy();
    }

    @Override
    public IBinder onBind(Intent intent) { return null; }
}
