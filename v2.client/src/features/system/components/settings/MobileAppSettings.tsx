import { useCallback, useEffect, useState } from 'react';
import { Alert, Box, Button, CircularProgress, Stack, Typography } from '@mui/material';
import { useTranslation } from 'react-i18next';
import { isAndroidApp } from '@/shared/platform';
import { checkForAppUpdate, type AppUpdate } from '../../services/appUpdate';
import { downloadAndInstallApk, onApkDownloadProgress } from '../../services/apkInstaller';
import AccountSettings from './AccountSettings';

const MobileAppSettings: React.FC<{ autoCheck?: boolean; showControls?: boolean }> = ({ autoCheck = false, showControls = true }) => {
  const { t } = useTranslation();
  const [update, setUpdate] = useState<AppUpdate | null>(null);
  const [checking, setChecking] = useState(false);
  const [checked, setChecked] = useState(false);
  const [error, setError] = useState(false);
  const [downloadError, setDownloadError] = useState<string | null>(null);
  const [downloadProgress, setDownloadProgress] = useState<number | null>(null);

  const downloadUpdate = useCallback(async (url: string) => {
    setError(false);
    setDownloadError(null);
    setDownloadProgress(0);
    let listener: Awaited<ReturnType<typeof onApkDownloadProgress>> | undefined;
    try {
      listener = await onApkDownloadProgress(progress => {
        const normalizedProgress = Math.max(0, Math.min(100, Math.floor(progress)));
        setDownloadProgress(current => current === null ? normalizedProgress : Math.max(current, normalizedProgress));
      });
      await downloadAndInstallApk(url);
    } catch (cause) {
      setDownloadProgress(null);
      const message = cause instanceof Error ? cause.message : String(cause);
      setDownloadError(message || t('settings.updateDownloadFailed'));
    } finally {
      await listener?.remove();
    }
  }, [t]);

  const check = useCallback(async (automatic = false) => {
    setChecking(true);
    setError(false);
    try {
      const result = await checkForAppUpdate();
      setUpdate(result);
      setChecked(true);
      if (automatic && result && window.confirm(t('settings.updatePrompt', { version: result.version }))) {
        void downloadUpdate(result.apkUrl);
      }
    } catch {
      setError(true);
    } finally {
      setChecking(false);
    }
  }, [downloadUpdate, t]);

  useEffect(() => {
    if (autoCheck) void check(true);
  }, [autoCheck, check]);

  if (!isAndroidApp) return null;

  if (!showControls) {
    return downloadProgress !== null ? (
      <Box
        role="status"
        aria-live="polite"
        sx={{
          position: 'fixed',
          inset: 0,
          zIndex: theme => theme.zIndex.modal + 1,
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          bgcolor: 'background.default',
          color: '#fff',
          p: 3,
        }}
      >
        <Stack spacing={2} alignItems="center">
          <Box sx={{ position: 'relative', display: 'inline-flex' }}>
            <CircularProgress variant="determinate" value={downloadProgress} size={64} />
            <Box
              sx={{
                position: 'absolute',
                inset: 0,
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
              }}
            >
              <Typography variant="caption" component="span" sx={{ color: '#fff' }}>{downloadProgress}%</Typography>
            </Box>
          </Box>
          <Typography variant="h6" sx={{ color: '#fff' }}>Downloading update</Typography>
        </Stack>
      </Box>
    ) : null;
  }

  return (
    <Stack spacing={1}>
      <Typography variant="h6">{t('settings.appUpdates')}</Typography>
      {checking && <CircularProgress size={22} />}
      {downloadProgress !== null && <Alert severity="info">Downloading update: {downloadProgress}%</Alert>}
      {!checking && checked && !error && !update && <Alert severity="success">{t('settings.upToDate')}</Alert>}
      {update && <Alert severity="info">{t('settings.updateAvailable', { version: update.version })}</Alert>}
      {error && <Alert severity="error">{t('settings.updateCheckFailed')}</Alert>}
      {downloadError && <Alert severity="error">{downloadError}</Alert>}
      <Button variant="outlined" onClick={() => void check()} disabled={checking}>
        {t('settings.checkForUpdates')}
      </Button>
      {update && <Button variant="contained" disabled={downloadProgress !== null} onClick={() => void downloadUpdate(update.apkUrl)}>
        {downloadProgress === null ? t('settings.downloadUpdate') : `${downloadProgress}%`}
      </Button>}
      <AccountSettings />
    </Stack>
  );
};

export default MobileAppSettings;
