import { Autocomplete, Box, TextField } from "@mui/material";
import { useTranslation } from "react-i18next";
import { useApiConnect } from "@/shared/context/apiConnect";
import { useEffect, useState } from "react";
import { Story } from "@/features/rpg";

const SummaryTaskForm = ({ task, onChange }) => {
  const { t } = useTranslation();

 const [rpgList, setRPGList] = useState<{label: string, value: string}[]>([]);
 

  const api = useApiConnect();

  const getListRPG = async() => {

    var stories = await api.get<Story[]>('rpg_stories_data');
    setRPGList(stories.data.map((story) => ({ label: story.title, value: story.id })));
  }

    // Always use updateData for initial load
    useEffect(() => {
        getListRPG();
    }, []);



  return (
    <Box display="flex" flexDirection="column" gap={2}>
      <Autocomplete
        disablePortal
        options={rpgList || []}
        sx={{ width: 300 }}
        onChange={(e, v) => onChange({ ...task, data: { ...task.data, summaryData: { id: v.value, title: v.label, } } })}
        renderInput={(params) => <TextField {...params} label={t("automations.jobs.summaryDataRPG")} />}
      />
    </Box>
  );
};

export default SummaryTaskForm;
