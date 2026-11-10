import { registerPlugin, type PluginListenerHandle } from '@capacitor/core';

interface ApkInstallerPlugin {
  downloadAndInstall(options: { url: string }): Promise<void>;
  addListener(eventName: 'downloadProgress', listener: (event: { progress: number }) => void): Promise<PluginListenerHandle>;
}

const ApkInstaller = registerPlugin<ApkInstallerPlugin>('ApkInstaller');

export function downloadAndInstallApk(url: string): Promise<void> {
  return ApkInstaller.downloadAndInstall({ url });
}

export function onApkDownloadProgress(listener: (progress: number) => void): Promise<PluginListenerHandle> {
  return ApkInstaller.addListener('downloadProgress', event => listener(event.progress));
}
