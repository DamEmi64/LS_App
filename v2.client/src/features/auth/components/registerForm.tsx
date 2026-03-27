import React, { useState } from 'react';
import { TextField, Box, Typography, Button, useTheme } from '@mui/material';
import { useTranslation } from 'react-i18next';

import { RegisterData } from '@/features/auth';
import { AuthContextType } from '@/features/auth/context/authProvider';

interface RegisterFormProps {
  auth: AuthContextType;
  onClose: () => void;
}

const RegisterForm: React.FC<RegisterFormProps> = ({ auth, onClose }) => {
  const { t } = useTranslation();
  const theme = useTheme();

  const textColor =
    theme.palette.mode === 'dark'
      ? theme.palette.text.primary
      : theme.palette.text.secondary;

  const [username, setUsername] = useState('');
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async () => {
    if (!username || !email || !password) return;

    setLoading(true);

    try {
      const registerData = {} as RegisterData;
      registerData.login = username;
      registerData.firstName = firstName;
      registerData.lastName = lastName;
      registerData.email = email;
      registerData.password = password;

      const success = await auth.register(registerData);

      if (success) {
        onClose();
      } else {
        console.error('Registration failed');
      }
    } catch (err) {
      console.error('Registration error:', err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <Typography variant="h6" gutterBottom sx={{ color: textColor }}>
        {t('register')}
      </Typography>

      <Box display="flex" flexDirection="column" gap={2}>
        <TextField
          label={t('auth.register.username')}
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          fullWidth
          autoComplete="username"
          InputLabelProps={{ sx: { color: textColor } }}
        />

        <TextField
          label={t('auth.register.firstName')}
          value={firstName}
          onChange={(e) => setFirstName(e.target.value)}
          fullWidth
          autoComplete="given-name"
          InputLabelProps={{ sx: { color: textColor } }}
        />

        <TextField
          label={t('auth.register.lastName')}
          value={lastName}
          onChange={(e) => setLastName(e.target.value)}
          fullWidth
          autoComplete="family-name"
          InputLabelProps={{ sx: { color: textColor } }}
        />

        <TextField
          label={t('auth.register.email')}
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          fullWidth
          autoComplete="email"
          InputLabelProps={{ sx: { color: textColor } }}
        />

        <TextField
          label={t('auth.register.password')}
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          fullWidth
          autoComplete="new-password"
          InputLabelProps={{ sx: { color: textColor } }}
        />

        <Box sx={{ display: 'flex', gap: 2, mt: 3 }}>
          <Button
            onClick={handleSubmit}
            variant="contained"
            disabled={loading || !username || !email || !password}
          >
            {t('opt.register')}
          </Button>
        </Box>
      </Box>
    </>
  );
};

export default RegisterForm;