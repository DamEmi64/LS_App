import React, { useEffect, useState } from "react";
import {
  Typography,
  Button,
  Grid,
  TextField,
  useTheme,
  Box
} from "@mui/material";
import { useTranslation } from "react-i18next";

import { User } from "@/features/auth";
import { AuthContextType } from "@/features/auth/context/authProvider";

export type UserEditProps = {
  auth: AuthContextType;
  onClose: () => void;
};

const UserEdit: React.FC<UserEditProps> = ({ auth, onClose }) => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(false);

  const { t } = useTranslation();
  const theme = useTheme();

  const textColor = theme.palette.text.primary;

  useEffect(() => {
    auth.getData().then((data) => setUser(data));
  }, [auth]);

  const handleChange = (field: keyof User, value: string) => {
    if (!user) return;
    setUser({ ...user, [field]: value });
  };

  const handleSubmit = async () => {
    if (!user) return;

    setLoading(true);

    try {
      const success = await auth.update(user);

      if (success) {
        onClose();
      } else {
        console.error("Update failed");
      }
    } catch (err) {
      console.error("Update error:", err);
    } finally {
      setLoading(false);
    }
  };

  if (!user) {
    return (
      <Typography sx={{ color: textColor }}>
        {t("common.loading")}
      </Typography>
    );
  }

  return (
    <Box>
      <Typography variant="h5" gutterBottom sx={{ color: textColor }}>
        {t("user.info")}
      </Typography>

      <Grid container spacing={2}>
        <Grid size={{ xs: 12, md: 6 }}>
          <TextField
            fullWidth
            label={t("user.firstName")}
            value={user.firstName ?? ""}
            onChange={(e) => handleChange("firstName", e.target.value)}
          />
        </Grid>

        <Grid size={{ xs: 12, md: 6 }}>
          <TextField
            fullWidth
            label={t("user.lastName")}
            value={user.lastName ?? ""}
            onChange={(e) => handleChange("lastName", e.target.value)}
          />
        </Grid>

        <Grid size={{ xs: 12, md: 6 }}>
          <TextField
            fullWidth
            label={t("user.email")}
            value={user.email ?? ""}
            onChange={(e) => handleChange("email", e.target.value)}
          />
        </Grid>

        <Grid size={{ xs: 12, md: 6 }}>
          <TextField
            fullWidth
            label={t("user.userName")}
            value={user.userName ?? ""}
            onChange={(e) => handleChange("userName", e.target.value)}
          />
        </Grid>

        <Grid size={{ xs: 12, md: 6 }}>
          <TextField
            fullWidth
            label={t("user.phoneNumber")}
            value={user.phoneNumber ?? ""}
            onChange={(e) => handleChange("phoneNumber", e.target.value)}
          />
        </Grid>
      </Grid>

      <Box sx={{ mt: 3 }}>
        <Button
          variant="contained"
          onClick={handleSubmit}
          disabled={loading}
        >
          {t("opt.save")}
        </Button>
      </Box>
    </Box>
  );
};

export default UserEdit;
