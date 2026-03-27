import { Box, Typography } from "@mui/material";
import { useTranslation } from "react-i18next";

const PermissionPage = () => {
  const { t } = useTranslation();

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

      <Typography sx={{ width: { xs: "90%", md: "40%" } }}>
        {t("auth.no_permission")}
      </Typography>
    </Box>
  );
};

export default PermissionPage;