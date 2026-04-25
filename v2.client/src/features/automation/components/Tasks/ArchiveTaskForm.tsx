import { Box, TextField } from "@mui/material";
import { useTranslation } from "react-i18next";

const ArchiveTaskForm = ({ task, onChange }) => {

  const { t } = useTranslation();
  return (
    <Box display="flex" flexDirection="column" gap={2}>
      <TextField
        label={t("automations.jobs.archive.sourceDir")}
        value={task.data.sourceDir || ''}
        onChange={(e) => onChange({ ...task, data: { ...task.data, sourceDir: e.target.value } })}
      />
      <TextField
              label={t("automations.jobs.archive.descDir")}
        value={task.data.destDir || ''}
        onChange={(e) => onChange({ ...task, data: { ...task.data, destDir: e.target.value } })}
      />
    </Box>
  );
};

export default ArchiveTaskForm;