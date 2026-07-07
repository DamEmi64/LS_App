import { useConfiguration } from "@/shared/context/configuration";
import { Box, Tab, Tabs, Typography } from "@mui/material";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import ServerSettings from "./serverSettings";
import GeneralSettings from "./generalSettings";
import NotificationSettings from "./NotificationSettings";

const SettingsWrapper: React.FC = () => {
    const { useVariable } = useConfiguration();
    const { t } = useTranslation();
    const [labelColor] = useVariable('labelColor');
    const tabs: { label: string, content: React.ReactNode }[] = [
        {
            label: t('settings.general'),
            content: (<GeneralSettings />)
        },
        {
            label: t('settings.server'),
            content: (<ServerSettings />)
        },
        {
            label: t('settings.notifications'),
            content: (<NotificationSettings />)
        },
    ];

    const [value, setValue] = useState(0);

    const handleChange = (event: React.SyntheticEvent, newValue: number) => {
        setValue(newValue);
    };

    return (
        <>
            <Typography variant="h6" gutterBottom sx={{ color: labelColor }}>
                {t('settings.title')}
            </Typography>
            <Box>
                <Tabs value={value} onChange={handleChange} variant="scrollable" allowScrollButtonsMobile>
                    {tabs.map((tab, index) => (
                        <Tab key={index} label={tab.label} id={`settings-tab-${index}`} />
                    ))}
                </Tabs>
                <Box sx={{ pt: 2 }}>
                    {tabs[value].content}
                </Box>
            </Box>
        </>
    );
};

export default SettingsWrapper;
