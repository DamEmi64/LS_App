import { Capacitor, registerPlugin } from '@capacitor/core';

type MeetingRecorderPlugin = {
    startRecording(options: { meetingId: string }): Promise<void>;
    stopRecording(): Promise<{ path: string }>;
    getRecordingState(): Promise<{ recording: boolean; path?: string | null; meetingId?: string | null }>;
    openRecording(options: { path: string }): Promise<void>;
    transcribeRecording(options: { path: string }): Promise<{ transcript: string }>;
    addListener(eventName: 'transcriptionProgress', listener: (data: { phase: 'download' | 'transcribing'; progress: number }) => void): Promise<{ remove: () => Promise<void> }>;
};

const MeetingRecorder = registerPlugin<MeetingRecorderPlugin>('MeetingRecorder');

export const isMeetingRecordingAvailable = () =>
    Capacitor.isNativePlatform() && Capacitor.getPlatform() === 'android';

export const startMeetingRecording = (meetingId: string) => {
    if (!isMeetingRecordingAvailable()) {
        throw new Error('Meeting recording is currently available in the Android app only.');
    }
    return MeetingRecorder.startRecording({ meetingId });
};

export const stopMeetingRecording = () => MeetingRecorder.stopRecording();

export const getMeetingRecordingState = () => MeetingRecorder.getRecordingState();

export const openMeetingRecording = (path: string) => MeetingRecorder.openRecording({ path });

export const transcribeMeetingRecording = async (
    path: string,
    onProgress?: (data: { phase: 'download' | 'transcribing'; progress: number }) => void
) => {
    const listener = onProgress
        ? await MeetingRecorder.addListener('transcriptionProgress', onProgress)
        : undefined;
    try {
        return await MeetingRecorder.transcribeRecording({ path });
    } finally {
        await listener?.remove();
    }
};
