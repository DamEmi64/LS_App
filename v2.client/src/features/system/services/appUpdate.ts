import { App } from '@capacitor/app';
import { isAndroidApp } from '@/shared/platform';

const latestReleaseUrl = 'https://api.github.com/repos/DamEmi64/LS_App/releases/latest';

export type AppUpdate = {
  version: string;
  apkUrl: string;
};

function compareVersions(left: string, right: string): number {
  const parse = (value: string) => value.replace(/^v/i, '').split(/[.+-]/).map(part => Number(part) || 0);
  const a = parse(left);
  const b = parse(right);

  for (let index = 0; index < Math.max(a.length, b.length); index += 1) {
    const difference = (a[index] ?? 0) - (b[index] ?? 0);
    if (difference !== 0) return difference;
  }

  return 0;
}

export async function checkForAppUpdate(): Promise<AppUpdate | null> {
  if (!isAndroidApp) return null;

  const response = await fetch(latestReleaseUrl, {
    headers: { Accept: 'application/vnd.github+json' },
  });
  if (!response.ok) throw new Error(`GitHub update check failed (${response.status})`);

  const release = await response.json();
  const apk = release.assets?.find((asset: { name: string }) => asset.name.toLowerCase() === 'app.apk');
  if (!apk?.browser_download_url) throw new Error('The latest GitHub release does not contain app.apk');

  const installed = await App.getInfo();
  const version = String(release.tag_name || '').replace(/^v/i, '');
  if (!version) throw new Error('The latest GitHub release has no version tag');
  if (compareVersions(version, installed.version) <= 0) return null;

  return { version, apkUrl: apk.browser_download_url };
}
