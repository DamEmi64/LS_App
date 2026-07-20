import { Box, FormControl, InputLabel, MenuItem, Select } from "@mui/material";
import { useTranslation } from "react-i18next";

const reminderTypes = [
  { value: "Min15", label: "15 minutes before" },
  { value: "Min30", label: "30 minutes before" },
  { value: "Day1", label: "1 day before" },
  { value: "Week1", label: "1 week before" },
  { value: "Month1", label: "1 month before" },
];

const SendReminderTaskForm = ({ task, onChange }) => {
  const { t } = useTranslation();
  const value = typeof task.data === "string" ? task.data : "Min15";

  return (
    <Box display="flex" flexDirection="column" gap={2} mt={2}>
      <FormControl fullWidth>
        <InputLabel>{t("automations.jobs.reminder.type")}</InputLabel>
        <Select
          value={value}
          label={t("automations.jobs.reminder.type")}
          onChange={(e) => onChange({ ...task, data: e.target.value })}
        >
          {reminderTypes.map((type) => (
            <MenuItem key={type.value} value={type.value}>
              {t(`automations.jobs.reminder.${type.value}`, type.label)}
            </MenuItem>
          ))}
        </Select>
      </FormControl>
    </Box>
  );
};

export default SendReminderTaskForm;
