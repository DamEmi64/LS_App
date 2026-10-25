import React, { useState } from "react";
import { Button, Stack, TextField, Typography } from "@mui/material";
import { useTranslation } from "react-i18next";
import { SystemAutomat } from "../types";

interface SystemConfigurationFormProps {
  automat: SystemAutomat;
  onSave: (configuration: string) => Promise<void>;
  onCancel: () => void;
}

export const SystemConfigurationForm: React.FC<SystemConfigurationFormProps> = ({ automat, onSave, onCancel }) => {
  const { t } = useTranslation();
  const [configuration, setConfiguration] = useState(() => {
    try {
      return JSON.stringify(JSON.parse(automat.configuration), null, 2);
    } catch {
      return automat.configuration;
    }
  });
  const [error, setError] = useState("");
  const [saving, setSaving] = useState(false);

  const submit = async (event: React.FormEvent) => {
    event.preventDefault();
    try {
      JSON.parse(configuration);
      setError("");
    } catch {
      setError(t("automations.system.invalidJson"));
      return;
    }

    setSaving(true);
    try {
      await onSave(configuration);
    } finally {
      setSaving(false);
    }
  };

  return (
    <form onSubmit={submit}>
      <Stack spacing={2}>
        <Typography variant="h6">{t("automations.system.editConfiguration", { key: automat.key })}</Typography>
        <TextField
          autoFocus
          multiline
          minRows={12}
          maxRows={24}
          fullWidth
          label={t("automations.system.configuration")}
          value={configuration}
          onChange={event => setConfiguration(event.target.value)}
          error={Boolean(error)}
          helperText={error || t("automations.system.jsonHint")}
          inputProps={{ spellCheck: false, style: { fontFamily: "monospace" } }}
        />
        <Stack direction="row" spacing={1} justifyContent="flex-end">
          <Button onClick={onCancel}>{t("opt.cancel")}</Button>
          <Button type="submit" variant="contained" disabled={saving}>{t("opt.save")}</Button>
        </Stack>
      </Stack>
    </form>
  );
};
