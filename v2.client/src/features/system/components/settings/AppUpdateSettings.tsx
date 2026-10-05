import { useCallback, useEffect, useState } from 'react';
import { Alert, Button, CircularProgress, Stack, Typography } from '@mui/material';
import { useTranslation } from 'react-i18next';
import { isAndroidApp } from '@/shared/platform';
import { checkForAppUpdate, type AppUpdate } from '../../services/appUpdate';
import { openApkDownload } from '../../services/apkInstaller';

const AppUpdateSettings: React.FC<{ autoCheck?: boolean; showControls?: boolean }> = ({ autoCheck = false, showControls = true }) => {
  const { t } = useTranslation();
  const [update, setUpdate] = useState<AppUpdate | null>(null);
  const [checking, setChecking] = useState(false);
  const [checked, setChecked] = useState(false);
  const [error, setError] = useState(false);

  const check = useCallback(async (automatic = false) => {
    setChecking(true);
    setError(false);
    try {
      const result = await checkForAppUpdate();
      setUpdate(result);
      setChecked(true);
      if (automatic && result && window.confirm(t('settings.updatePrompt', { version: result.version }))) {
        await openApkDownload(result.apkUrl);
      }
    } catch {
      setError(true);
    } finally {
      setChecking(false);
    }
  }, [t]);

  useEffect(() => {
    if (autoCheck) void check(true);
  }, [autoCheck, check]);

  if (!isAndroidApp || !showControls) return null;

  return (
    <Stack spacing={1}>
      <Typography variant="h6">{t('settings.appUpdates')}</Typography>
      {checking && <CircularProgress size={22} />}
      {!checking && checked && !error && !update && <Alert severity="success">{t('settings.upToDate')}</Alert>}
      {update && <Alert severity="info">{t('settings.updateAvailable', { version: update.version })}</Alert>}
      {error && <Alert severity="error">{t('settings.updateCheckFailed')}</Alert>}
      <Button variant="outlined" onClick={() => void check()} disabled={checking}>
        {t('settings.checkForUpdates')}
      </Button>
      {update && <Button variant="contained" onClick={() => void openApkDownload(update.apkUrl).catch(() => setError(true))}>
        {t('settings.downloadUpdate')}
      </Button>}
    </Stack>
  );
};

export default AppUpdateSettings;
