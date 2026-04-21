import React, { useState } from 'react';
import {
  TextField,
  FormControlLabel,
  Checkbox,
  Box,
  Typography,
  Button,
  useColorScheme
} from '@mui/material';
import { useTranslation } from 'react-i18next';
import { LoginFormProps, LoginData } from '@/features/auth';
import {notify} from "@/shared";


const LoginForm: React.FC<LoginFormProps> = ({ auth, onClose }) => {
  const { t } = useTranslation();
  const { mode } = useColorScheme();

  const labelColor = mode === 'dark' ? '#fff' : '#000';

  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(false);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async () => {
    if (!username || !password) return;

    setLoading(true);

    try {
      const loginData = {} as LoginData;
      loginData.login = username;
      loginData.password = password;
      loginData.rememberMe = rememberMe;

      const success = await auth.login(loginData);

      if (success) {
        onClose();
      } else {
        notify('error', t('auth.login.failed'));
      }
    } catch (err) {
        notify('error', err instanceof Error ? err.message : t('auth.login.failed'));
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <Typography variant="h6" gutterBottom sx={{ color: labelColor }}>
        {t('login')}
      </Typography>

      <Box display="flex" flexDirection="column" gap={2}>
        <TextField
          label={t('auth.login.username')}
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          fullWidth
          autoComplete="username"
          InputLabelProps={{ sx: { color: labelColor } }}
        />

        <TextField
          label={t('auth.login.password')}
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          fullWidth
          autoComplete="current-password"
          InputLabelProps={{ sx: { color: labelColor } }}
        />

        <FormControlLabel
          control={
            <Checkbox
              checked={rememberMe}
              onChange={(e) => setRememberMe(e.target.checked)}
              color="primary"
            />
          }
          label={t('auth.login.rememberMe')}
          sx={{ color: labelColor }}
        />

        <Box sx={{ display: 'flex', gap: 2, mt: 3 }}>
          <Button
            onClick={handleSubmit}
            variant="contained"
            disabled={loading || !username || !password}
          >
            {t('opt.login')}
          </Button>
        </Box>
      </Box>
    </>
  );
};

export default LoginForm;