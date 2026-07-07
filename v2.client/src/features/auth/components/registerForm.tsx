import React, { useState } from 'react';
import { Box, Typography, Button, TextField, useTheme, Grid } from '@mui/material';
import { Controller, useForm } from 'react-hook-form';
import { useTranslation } from 'react-i18next';

import { RegisterData } from '@/features/auth';
import { AuthContextType } from '@/features/auth/context/authProvider';
import { notify } from '@/shared';
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';

interface RegisterFormProps {
  auth: AuthContextType;
  onClose: () => void;
}

const RegisterForm: React.FC<RegisterFormProps> = ({ auth, onClose }) => {
  const { t } = useTranslation();
  const theme = useTheme();
  const [showPassword, setShowPassword] = useState(false);
  
  const textColor = theme.palette.text.primary;

  const {
    control,
    handleSubmit,
    formState: { errors, isSubmitting, isValid },
  } = useForm<RegisterData>({
    mode: 'onChange',
    defaultValues: {
      login: '',
      firstName: '',
      lastName: '',
      email: '',
      password: '',
    },
  });

  const onSubmit = async (data: RegisterData) => {
    try {
      const success = await auth.register(data);

      if (success) {
        onClose();
      } else {
        notify('error', t('auth.register.failed'));
      }
    } catch (err) {
      notify('error', err instanceof Error ? err.message : t('auth.register.failed'));
    }
  };

  return (
    <>
      <Typography variant="h6" gutterBottom sx={{ color: textColor }}>
        {t('register')}
      </Typography>

      <Box
        component="form"
        display="flex"
        flexDirection="column"
        gap={2}
        onSubmit={handleSubmit(onSubmit)}
      >
        <Controller
          name="login"
          control={control}
          rules={{
            required: t('validation.required'),
            minLength: {
              value: 3,
              message: t('validation.minLength'),
            },
          }}
          render={({ field }) => (
            <TextField
              {...field}
              label={t('auth.register.username')}
              fullWidth
              autoComplete="username"
              error={!!errors.login}
              helperText={errors.login?.message}
              InputLabelProps={{ sx: { color: textColor } }}
            />
          )}
        />

        <Controller
          name="firstName"
          control={control}
          render={({ field }) => (
            <TextField
              {...field}
              label={t('auth.register.firstName')}
              fullWidth
              autoComplete="given-name"
              error={!!errors.firstName}
              helperText={errors.firstName?.message}
              InputLabelProps={{ sx: { color: textColor } }}
            />
          )}
        />

        <Controller
          name="lastName"
          control={control}
          render={({ field }) => (
            <TextField
              {...field}
              label={t('auth.register.lastName')}
              fullWidth
              autoComplete="family-name"
              error={!!errors.lastName}
              helperText={errors.lastName?.message}
              InputLabelProps={{ sx: { color: textColor } }}
            />
          )}
        />

        <Controller
          name="email"
          control={control}
          rules={{
            required: t('validation.required'),
            pattern: {
              value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
              message: t('validation.invalidEmail'),
            },
          }}
          render={({ field }) => (
            <TextField
              {...field}
              label={t('auth.register.email')}
              type="email"
              fullWidth
              autoComplete="email"
              error={!!errors.email}
              helperText={errors.email?.message}
              InputLabelProps={{ sx: { color: textColor } }}
            />
          )}
        />

        <Controller
          name="password"
          control={control}
          rules={{
            required: t('validation.required'),
            minLength: {
              value: 6,
              message: t('validation.minLength', { count: 6 }),
            },
          }}
          render={({ field }) => (
            <>
              <TextField
                {...field}
                label={t('auth.register.password')}
                type={showPassword ? "text" : "password"}
                fullWidth
                autoComplete="new-password"
                error={!!errors.password}
                helperText={errors.password?.message}
                InputLabelProps={{ sx: { color: textColor } }}
              />
              <Button
                onClick={() => setShowPassword(!showPassword)}
                variant="outlined"
                size="small"
              >
                {showPassword ? <VisibilityIcon /> : <VisibilityOffIcon />}
              </Button>
            </>
          )}
        />

        <Box sx={{ display: 'flex', gap: 2, mt: 3 }}>
          <Button type="submit" variant="contained" disabled={isSubmitting || !isValid}>
            {t('opt.register')}
          </Button>
        </Box>
      </Box>
    </>
  );
};

export default RegisterForm;
