import { useEffect, useState } from "react";
import { Alert, Button, Dialog, DialogActions, DialogContent, DialogTitle, FormControl, InputLabel, MenuItem, Select } from "@mui/material";
import { useTranslation } from "react-i18next";
import type { UserData } from "@/features/auth";
import { loadEventUsers } from "../services/eventService";

type AdminSignInDialogProps = {
  open: boolean;
  onClose: () => void;
  onSubmit: (user: UserData) => Promise<void>;
};

const AdminSignInDialog = ({ open, onClose, onSubmit }: AdminSignInDialogProps) => {
  const { t } = useTranslation();
  const [users, setUsers] = useState<UserData[]>([]);
  const [userId, setUserId] = useState("");
  const [loading, setLoading] = useState(false);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    if (!open) return;
    let active = true;
    setLoading(true);
    setError("");
    loadEventUsers()
      .then(data => { if (active) setUsers(data); })
      .catch(() => { if (active) setError(t("events.usersLoadFailed")); })
      .finally(() => { if (active) setLoading(false); });
    return () => { active = false; };
  }, [open, t]);

  const submit = async () => {
    const selected = users.find(user => user.userId === userId);
    if (!selected) return;
    setSaving(true);
    try {
      await onSubmit(selected);
      setUserId("");
      onClose();
    } catch {
      setError(t("events.adminSignInFailed"));
    } finally {
      setSaving(false);
    }
  };

  return (
    <Dialog open={open} onClose={saving ? undefined : onClose} fullWidth maxWidth="sm">
      <DialogTitle>{t("events.adminSignIn")}</DialogTitle>
      <DialogContent>
        {error && <Alert severity="error" sx={{ mb: 1 }}>{error}</Alert>}
        <FormControl fullWidth margin="dense" disabled={loading || saving}>
          <InputLabel>{t("events.selectUser")}</InputLabel>
          <Select value={userId} label={t("events.selectUser")} onChange={event => setUserId(event.target.value)}>
            {users.map(user => (
              <MenuItem key={user.userId} value={user.userId}>
                {user.login || user.email || user.userId}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} disabled={saving}>{t("opt.cancel")}</Button>
        <Button variant="contained" onClick={submit} disabled={!userId || loading || saving}>
          {t("events.signIn")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default AdminSignInDialog;
