import React, { useState } from 'react';
import {
  TextField,
  FormControlLabel,
  Checkbox,
  Box,
  Typography,
  Button,
  useColorScheme,
  Grid
} from '@mui/material';
import { useTranslation } from 'react-i18next';
import { LoginFormProps, LoginData } from '@/features/auth';
import {notify, useModal} from "@/shared";
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import ResetPasswordForm from './resetPasswordForm';


const LoginForm: React.FC<LoginFormProps> = ({ auth, onClose }) => {
  const { t } = useTranslation();
  const { mode } = useColorScheme();
  const modal = useModal();

  const labelColor = mode === 'dark' ? '#fff' : '#000';

  const [username, setUsername] = useState(localStorage.getItem('rememberedUsername') || '');
  const [password, setPassword] = useState(localStorage.getItem('rememberedPassword') || '');
  const [rememberMe, setRememberMe] = useState(false);
  const [loading, setLoading] = useState(false);
  const [loginFailed, setLoginFailed] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

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

        if (rememberMe) {
          localStorage.setItem('rememberedUsername', username);
          localStorage.setItem('rememberedPassword', password);
        }

        onClose();
      } else {
        setLoginFailed(true);
        notify('error', t('auth.login.failed'));
      }
    } catch (err) {
        notify('error', err instanceof Error ? err.message : t('auth.login.failed'));
    } finally {
      setLoading(false);
    }
  };

  const handleForgotPassword = () => {
    if (!username) return;

    modal.showSubModal(
      <ResetPasswordForm
        auth={auth}
        login={username}
        onClose={modal.hideSubModal}
      />
    );
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
          error={loginFailed}
          InputLabelProps={{ sx: { color: labelColor } }}
        />

        <Grid display="flex" flexDirection="row" gap={1}>
          <TextField
            label={t('auth.login.password')}
            type= {showPassword ? 'text' : 'password'}
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            fullWidth
            autoComplete="current-password"
            error={loginFailed}
            InputLabelProps={{ sx: { color: labelColor } }}
          />
          <Button
            onClick={() => setShowPassword(!showPassword)}
            variant="outlined"
            size="small"
          >
            {showPassword ? <VisibilityIcon /> : <VisibilityOffIcon />}
          </Button>
        </Grid>


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

        <Button
          onClick={handleForgotPassword}
          variant="text"
          disabled={!username}
          sx={{ alignSelf: 'flex-start' }}
        >
          {t('auth.forgotPassword', 'Forgot password?')}
        </Button>

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
