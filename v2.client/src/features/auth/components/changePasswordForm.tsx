import React, { useState } from 'react';
import { TextField, Box, Typography, Button, useTheme } from '@mui/material';
import { useTranslation } from 'react-i18next';
import { PasswordChangeData } from '@/features/auth';
import { AuthContextType } from '@/features/auth/context/authProvider';

interface ChangePasswordFormProps {
  auth: AuthContextType;
  onClose: () => void;
}

const ChangePasswordForm: React.FC<ChangePasswordFormProps> = ({ auth, onClose }) => {
  const { t } = useTranslation();
  const theme = useTheme();

  const labelColor =
    theme.palette.mode === 'dark'
      ? theme.palette.text.primary
      : theme.palette.text.secondary;

  const [oldPassword, setOldPassword] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async () => {
    if (!oldPassword || !password) return;

    setLoading(true);

    try {
      const data = {} as PasswordChangeData;
      data.oldPassword = oldPassword;
      data.newPassword = password;

      const success = await auth.changePassword(data);

      if (success) {
        onClose();
      } else {
        console.error('Password change failed');
      }
    } catch (err) {
      console.error('Error changing password:', err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <Typography variant="h6" gutterBottom sx={{ color: labelColor }}>
        {t('auth.change_password')}
      </Typography>

      <Box display="flex" flexDirection="column" gap={2}>
        <TextField
          label={t('auth.old_password')}
          type="password"
          value={oldPassword}
          onChange={(e) => setOldPassword(e.target.value)}
          fullWidth
          InputLabelProps={{ sx: { color: labelColor } }}
        />

        <TextField
          label={t('auth.new_password')}
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          fullWidth
          InputLabelProps={{ sx: { color: labelColor } }}
        />

        <Box sx={{ display: 'flex', gap: 2, mt: 3 }}>
          <Button
            variant="contained"
            onClick={handleSubmit}
            disabled={loading || !oldPassword || !password}
          >
            {t('opt.change_pass')}
          </Button>

          <Button variant="outlined" onClick={onClose}>
            {t('common.cancel')}
          </Button>
        </Box>
      </Box>
    </>
  );
};

export default ChangePasswordForm;