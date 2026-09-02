import React, { useEffect, useState } from 'react';
import { TextField, FormControlLabel, Switch, Select, MenuItem, InputLabel, FormControl, Box, Typography, useColorScheme, IconButton, Grid, Button } from '@mui/material';
import { changeLanguage } from 'i18next';
import { useTranslation } from 'react-i18next';
import ServerInfo, { ServerInfoProps } from '../serverInfo';
import { call } from '@/shared';
import { useConfiguration } from '@/shared/context/configuration';
import RefreshIcon from '@mui/icons-material/Refresh';
import { useAppStorage } from '@/shared/storage/useAppStorage';

const languages = [
    { code: 'en', label: 'English' },
    { code: 'pl', label: 'Polski' },
    { code: 'fr', label: 'French' },
    { code: 'de', label: 'German' }
    // Add more languages as needed
];

const AppSettings: React.FC = () => {
    const { useVariable } = useConfiguration();
    const [frontendVersion, setFrontendVersion] = useVariable('version');
    const { t, i18n } = useTranslation();
    const [endpoint, setEndpoint] = useVariable('apiEndpoint');
    const [language, setLanguage] = useAppStorage('lang', i18n.language || 'en');
    const [labelColor, setLabelColor] = useVariable('labelColor')
    const { mode, setMode } = useColorScheme();
    const [darkTheme, setDarkTheme] = useState(mode === 'dark');
    const [serverData, setServerData] = useState<ServerInfoProps>({ frontendVersion: 'unknown', version: 'unknown', modules: [] });
    const [position, setPosition] = useVariable('toastPosition');
    const [autoClose, setAutoCLose] = useVariable('toastAutoClose');
    const [active, setActive] = useVariable('toastActive');
    const [process, setProcess] = useVariable('toastProcess');
    const [processError, setProcessError] = useVariable('toastProcessError');

    // Always use updateData for initial load
    useEffect(() => {
        call<ServerInfoProps>(api => api.homeApi.get, {}).then(data => {
            data.frontendVersion = frontendVersion;
            setServerData(data);
        });
    }, []);

    const setTranslation = (lang: string) => {
        changeLanguage(lang);
        setLanguage(lang);
    };

    const onEndpointChange = (data: string) => {
        setEndpoint(data);
        refreshConnection();
    }

    const refreshConnection = () => {
        call<ServerInfoProps>(api => api.homeApi.get, {}).then(data => {
            data.frontendVersion = frontendVersion;
            setServerData(data);
        });
    }

    const setTheme = (isDark: boolean) => {
        setDarkTheme(isDark);
        if (isDark) {
            setMode('dark');
            setLabelColor('#fff');
        }
        else {
            setMode('light');
            setLabelColor('#000');
        }
    }

    return (
        <>
            <Typography variant="h6" gutterBottom sx={{ color: labelColor }}>
                {t('settings.title')}
            </Typography>
            <Box display="flex" flexDirection="column" gap={2}>
                <Grid display="flex" flexDirection="row" gap={1}>
                    <TextField
                        fullWidth
                        label={t('apiEndpoint')}
                        value={endpoint}
                        onChange={e => onEndpointChange(e.target.value)}
                        InputLabelProps={{ sx: { color: labelColor } }}
                    />
                    <Button onClick={() => refreshConnection()}
                        variant="outlined"
                        size="small">
                        <RefreshIcon />
                    </Button>
                </Grid>
                <FormControlLabel
                    control={
                        <Switch
                            checked={darkTheme}
                            onChange={e => setTheme(e.target.checked)}
                            color="primary"
                        />
                    }
                    label={t('darkTheme')}
                    sx={{ color: labelColor }}
                />
                <Box>
                    <Typography variant="h4" gutterBottom sx={{ color: labelColor }}>
                        {t('notify.notification')}
                    </Typography>
                    <FormControlLabel
                        control={
                            <Switch
                                checked={active == 'true'}
                                onChange={e => setActive(e.target.checked.toString())}
                                color="primary"
                            />
                        }
                        label={t('notify.active')}
                        sx={{ color: labelColor }}
                    />
                    <br />
                    {(active == 'true' && (
                        <FormControl fullWidth>
                                <TextField
                                    type='number'
                                    value={autoClose}
                                    onChange={e => setAutoCLose(e.target.value)}
                                    color="primary"
                                    label={t('notify.autoClose')}
                                />
                        </FormControl>
                    )
                    )}
                    <br />
                    <br />
                    <FormControl fullWidth>
                        <InputLabel id="language-select-label" sx={{ color: labelColor }}>
                            {t('notify.position')}
                        </InputLabel>
                        <Select
                            labelId="language-select-label"
                            value={position}
                            label={t('language')}
                            onChange={e => setPosition(e.target.value as string)}
                            sx={{ color: labelColor }}
                        >
                            <MenuItem key={'bottom-left'} value={'bottom-left'}>{t('notify.bottomLeft')}</MenuItem>
                            <MenuItem key={'top-right'} value={'top-right'}>{t('notify.topRight')}</MenuItem>
                            <MenuItem key={'bottom-right'} value={'bottom-right'}>{t('notify.bottomRight')}</MenuItem>
                            <MenuItem key={'top-left'} value={'top-left'}>{t('notify.topLeft')}</MenuItem>
                        </Select>
                    </FormControl>

                </Box>
                <FormControl fullWidth>
                    <InputLabel id="language-select-label" sx={{ color: labelColor }}>
                        {t('language')}
                    </InputLabel>
                    <Select
                        labelId="language-select-label"
                        value={language}
                        label={t('language')}
                        onChange={e => setTranslation(e.target.value as string)}
                        sx={{ color: labelColor }}
                    >
                        {languages.map(lang => (
                            <MenuItem key={lang.code} value={lang.code}>
                                {lang.label}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>
                <ServerInfo frontendVersion={frontendVersion} version={serverData.version} modules={serverData.modules || []}></ServerInfo>
            </Box >
        </>
    );
};

export default AppSettings;
