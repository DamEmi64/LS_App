import React, { useEffect, useState } from 'react';
import { TextField, Box, Typography, Button, useTheme } from '@mui/material';
import { useTranslation } from 'react-i18next';
import { ResetPasswordData } from '@/features/auth';
import { AuthContextType } from '@/features/auth/context/authProvider';
import { notify } from '@/shared';

interface ResetPasswordFormProps {
  auth: AuthContextType;
  login: string;
  onClose: () => void;
}

const ResetPasswordForm: React.FC<ResetPasswordFormProps> = ({ auth, login, onClose }) => {
  const { t } = useTranslation();
  const theme = useTheme();

  const labelColor =
    theme.palette.mode === 'dark'
      ? theme.palette.text.primary
      : theme.palette.text.secondary;

  const [code, setCode] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [sendCodeLoading, setSendCodeLoading] = useState(false);
  const [nextCodeRequestAt, setNextCodeRequestAt] = useState<number | null>(null);
  const [secondsToNextCode, setSecondsToNextCode] = useState(0);
  const [codeSent, setCodeSent] = useState(false);

  useEffect(() => {
    if (!nextCodeRequestAt) {
      setSecondsToNextCode(0);
      return;
    }

    const updateSeconds = () => {
      const seconds = Math.max(0, Math.ceil((nextCodeRequestAt - Date.now()) / 1000));
      setSecondsToNextCode(seconds);

      if (seconds === 0) {
        setNextCodeRequestAt(null);
      }
    };

    updateSeconds();
    const interval = window.setInterval(updateSeconds, 250);

    return () => window.clearInterval(interval);
  }, [nextCodeRequestAt]);

  const handleSendCode = async () => {
    if (!login || secondsToNextCode > 0) return;

    setSendCodeLoading(true);

    try {
      const success = await auth.forgotPassword(login);

      if (success) {
        setCodeSent(true);
        setNextCodeRequestAt(Date.now() + 5000);
      } else {
        notify('error', t('auth.forgot_password_send_failed', 'Could not send reset code'));
      }
    } catch (err) {
      notify('error', err instanceof Error ? err.message : t('auth.forgot_password_send_failed', 'Could not send reset code'));
    } finally {
      setSendCodeLoading(false);
    }
  };

  const handleSubmit = async () => {
    if (!login || !code || !password) return;

    setLoading(true);

    try {
      const data = {
        login,
        code,
        password,
      } as ResetPasswordData;

      const success = await auth.resetPassword(data);

      if (success) {
        notify('success', t('auth.reset_password_success', 'Password has been reset'));
        onClose();
      } else {
        notify('error', t('auth.reset_password_failed', 'Password reset failed'));
      }
    } catch (err) {
      notify('error', err instanceof Error ? err.message : t('auth.reset_password.failed', 'Password reset failed'));
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <Typography variant="h6" gutterBottom sx={{ color: labelColor }}>
        {t('auth.reset_password', 'Reset password')}
      </Typography>
      {codeSent ? (<Typography gutterBottom sx={{ color: labelColor }}>
          {t('auth.forgot_info')}
        </Typography>) : (<Typography gutterBottom sx={{ color: labelColor }}>
          {t('auth.forgot_info_initial')}
        </Typography>)}
      <Box display="flex" flexDirection="column" gap={2}>
        <TextField
          label={t('auth.login.username')}
          value={login}
          fullWidth
          disabled
          InputLabelProps={{ sx: { color: labelColor } }}
        />

        <Button
          variant="outlined"
          onClick={handleSendCode}
          disabled={sendCodeLoading || !login || secondsToNextCode > 0}
        >
          {secondsToNextCode > 0
            ? t('auth.forgot_password_wait', { seconds: secondsToNextCode })
            : t('auth.forgot_password_send_code')}
        </Button>

        <TextField
          label={t('auth.reset_password_code', 'Code')}
          value={code}
          onChange={(e) => setCode(e.target.value)}
          fullWidth
          autoComplete="one-time-code"
          InputLabelProps={{ sx: { color: labelColor } }}
        />

        <TextField
          label={t('auth.new_password', 'New password')}
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          fullWidth
          autoComplete="new-password"
          InputLabelProps={{ sx: { color: labelColor } }}
        />

        <Box sx={{ display: 'flex', gap: 2, mt: 3 }}>
          <Button
            variant="contained"
            onClick={handleSubmit}
            disabled={loading || !code || !password}
          >
            {t('opt.save')}
          </Button>

          <Button variant="outlined" onClick={onClose}>
            {t('opt.cancel')}
          </Button>
        </Box>
      </Box>
    </>
  );
};

export default ResetPasswordForm;
