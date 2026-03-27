import React, { useEffect, useState } from "react";
import {
  Typography,
  Button,
  Grid,
  useTheme,
  TextField,
  Box
} from "@mui/material";
import { useTranslation } from "react-i18next";

import { User } from "@/features/auth";
import { AuthContextType } from "@/features/auth/context/authProvider";

export type UserProfileProps = {
  auth: AuthContextType;
  onEdit: (user: User) => void;
  onChangePassword: (user: User) => void;
};

const UserProfile: React.FC<UserProfileProps> = ({
  auth,
  onEdit,
  onChangePassword
}) => {
  const [user, setUser] = useState<User | null>(null);

  const { t } = useTranslation();
  const theme = useTheme();

  const textColor =
    theme.palette.mode === "dark"
      ? theme.palette.text.primary
      : theme.palette.text.secondary;

  useEffect(() => {
    auth.getData().then((data) => setUser(data));
  }, [auth]);

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
            label={t("user.firstName")}
            value={user.firstName ?? ""}
            fullWidth
            InputProps={{ readOnly: true, style: { color: textColor } }}
            margin="dense"
          />
        </Grid>

        <Grid size={{ xs: 12, md: 6 }}>
          <TextField
            label={t("user.lastName")}
            value={user.lastName ?? ""}
            fullWidth
            InputProps={{ readOnly: true, style: { color: textColor } }}
            margin="dense"
          />
        </Grid>

        <Grid size={{ xs: 12, md: 6 }}>
          <TextField
            label={t("user.email")}
            value={user.email ?? ""}
            fullWidth
            InputProps={{ readOnly: true, style: { color: textColor } }}
            margin="dense"
          />
        </Grid>

        <Grid size={{ xs: 12, md: 6 }}>
          <TextField
            label={t("user.userName")}
            value={user.userName ?? ""}
            fullWidth
            InputProps={{ readOnly: true, style: { color: textColor } }}
            margin="dense"
          />
        </Grid>

        <Grid size={{ xs: 12, md: 6 }}>
          <TextField
            label={t("user.phoneNumber")}
            value={user.phoneNumber ?? ""}
            fullWidth
            InputProps={{ readOnly: true, style: { color: textColor } }}
            margin="dense"
          />
        </Grid>

        <Grid size={{ xs: 12 }}>
          <Box sx={{ display: "flex", gap: 2, mt: 2 }}>
            <Button
              variant="contained"
              onClick={() => onEdit(user)}
            >
              {t("opt.edit")}
            </Button>

            <Button
              variant="outlined"
              onClick={() => onChangePassword(user)}
            >
              {t("opt.change_pass")}
            </Button>
          </Box>
        </Grid>
      </Grid>
    </Box>
  );
};

export default UserProfile;