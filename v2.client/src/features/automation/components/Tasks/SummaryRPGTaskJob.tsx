import { Autocomplete, Box, TextField } from "@mui/material";
import { useTranslation } from "react-i18next";
import { useEffect, useState } from "react";
import { Story } from "@/features/rpg";
import { ResponseList } from "@/shared/api/extension";
import {call} from "@/shared";

const SummaryTaskForm = ({ task, onChange }) => {
  const { t } = useTranslation();

 const [rpgList, setRPGList] = useState<{label: string, value: string}[]>([]);
 

  const getListRPG = async() => {

    const stories = await call<ResponseList<Story>>(api => api.storiesApi.get,{order:''})
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
