import { registerPlugin } from '@capacitor/core';

interface ApkInstallerPlugin {
  openDownload(options: { url: string }): Promise<void>;
}

const ApkInstaller = registerPlugin<ApkInstallerPlugin>('ApkInstaller');

export function openApkDownload(url: string): Promise<void> {
  return ApkInstaller.openDownload({ url });
}
