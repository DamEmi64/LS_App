import { useTranslation } from "react-i18next";
import { useApiConnect } from "@/shared/context/apiConnect";
import { useEffect, useState } from "react";
import { Story } from "@/features/rpg";

const SummaryLastRPGTaskForm = ({ task, onChange }) => {
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
    <></>
  );
};

export default SummaryLastRPGTaskForm;
