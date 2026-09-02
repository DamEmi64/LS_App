import { Box, Button, Grid, Typography } from "@mui/material";
import { useTranslation } from "react-i18next";

import LoginForm from "@/features/auth/components/loginForm";
import RegisterForm from "@/features/auth/components/registerForm";

import { useModal } from "@/shared/context/modal";
import { useAuth } from "@/features/auth/context/authProvider";
import { isNativeApp } from '@/shared/platform';
import SettingsWrapper from '@/features/system/components/settings/SettingsWrapper';

const AuthPage = () => {
  const modal = useModal();
  const auth = useAuth();
  const { t } = useTranslation();

  const showLogin = () => {
    modal.showModal(<LoginForm onClose={modal.hideModal} auth={auth} />);
  };

  const showRegister = () => {
    modal.showModal(<RegisterForm onClose={modal.hideModal} auth={auth} />);
  };

  if (isNativeApp) {
    return (
      <Box sx={{ width: "100%", minHeight: "100vh", display: "flex", justifyContent: "center", alignItems: "center", p: 3 }}>
        <Button variant="contained" onClick={() => modal.showModal(<SettingsWrapper />)}>
          {t('menu.settings')}
        </Button>
      </Box>
    );
  }

  return (
    <Box
      sx={{
        width: "100%",
        minHeight: "100vh",
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        p: 3,
        textAlign: "center",
      }}
    >
      <Typography
        variant="h3"
        sx={{
          fontWeight: "bold",
          mb: 2,
        }}
      >
        {t("auth.welcome")}
      </Typography>

      <Typography
        sx={{
          width: { xs: "90%", md: "40%" },
          mb: 4,
        }}
      >
        {t("auth.info")}
      </Typography>

      <Grid
        container
        spacing={2}
        direction="column"
        alignItems="center"
        sx={{ width: { xs: "80%", md: "40%" } }}
      >
        <Grid>
          <Button variant="contained" onClick={showLogin}>
            {t("opt.login")}
          </Button>
        </Grid>

        <Grid>
          <Button variant="contained" onClick={showRegister}>
            {t("opt.register")}
          </Button>
        </Grid>
      </Grid>
    </Box>
  );
};

export default AuthPage;
