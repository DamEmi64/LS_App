import React, { useEffect, useState } from 'react';
import { TextField, FormControlLabel, Switch, Select, MenuItem, InputLabel, FormControl, Box, Typography, useColorScheme } from '@mui/material';
import { changeLanguage } from 'i18next';
import { useTranslation } from 'react-i18next';
import { ApiConnectContextType } from '@/shared/context/apiConnect';
import useLocalStorage from 'react-use-localstorage';
import ServerInfo, { ServerInfoProps } from './serverInfo';

const languages = [
    { code: 'en', label: 'English' },
    { code: 'pl', label: 'Polski' },
    { code: 'fr', label: 'French' },
    {code: 'de', label: 'German'}
    // Add more languages as needed
];

const AppSettings: React.FC<{ api: ApiConnectContextType }> = ({ api }) => {
    const [frontendVersion, setFrontendVersion] = useState('v1.0');

    const { t, i18n } = useTranslation();
    const [endpoint, setEndpoint] = useLocalStorage('apiEndpoint', 'http://localhost:5144');
    const [language, setLanguage] = useLocalStorage('lang', i18n.language || 'en');
    const [labelColor, setLabelColor] = useLocalStorage('labelColor', '#fff')
    const { mode, setMode } = useColorScheme();
    const [darkTheme, setDarkTheme] = useState(mode === 'dark');
    const [serverData, setServerData] = useState<ServerInfoProps>({ frontendVersion: 'unknown', version: 'unknown', modules: [] });
    const [position, setPosition] = useLocalStorage('toastPosition', "bottom-right");
    const [autoClose, setAutoCLose] = useLocalStorage('toastAutoClose', '3000');
    const [active, setActive] = useLocalStorage('toastActive', 'true');
    const [process, setProcess] = useLocalStorage('toastProcess', 'false');
    const [processError, setProcessError] = useLocalStorage('toastProcessError', 'false');

    // Always use updateData for initial load
    useEffect(() => {
        api.get<ServerInfoProps>('api_info').then(data => {
            data.data.frontendVersion = frontendVersion;
            setServerData(data.data);
        });
    }, []);

    const setTranslation = (lang: string) => {
        changeLanguage(lang);
        setLanguage(lang);
    };

    const onEndpointChange = (data: string) => {
        setEndpoint(data);
        api.get<ServerInfoProps>('api_info').then(data => {
            data.data.frontendVersion = frontendVersion;
            setServerData(data.data);
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
                {t('settings')}
            </Typography>
            <Box display="flex" flexDirection="column" gap={2}>
                <TextField
                    label={t('apiEndpoint')}
                    value={endpoint}
                    onChange={e => onEndpointChange(e.target.value)}
                    fullWidth
                    InputLabelProps={{ sx: { color: labelColor } }}
                />
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
                        <>
                            <FormControlLabel
                                control={
                                    <Switch
                                        checked={process == 'true'}
                                        onChange={e => setProcess(e.target.checked.toString())}
                                        color="primary"
                                    />
                                }
                                label={t('notify.processActive')}
                                sx={{ color: labelColor }}
                            />
                            {(process == 'true' && (
                                <>
                                    <br />
                                    <FormControlLabel
                                        control={
                                            <Switch
                                                checked={processError == 'true'}
                                                onChange={e => setProcessError(e.target.checked.toString())}
                                                color="primary"
                                            />
                                        }
                                        label={t('notify.processError')}
                                        sx={{ color: labelColor }}
                                    />
                                </>

                            )
                            )}

                            <br />
                            <FormControlLabel
                                control={
                                    <TextField
                                        type='number'
                                        value={autoClose}
                                        onChange={e => setAutoCLose(e.target.value)}
                                        color="primary"
                                    />
                                }
                                label={t('notify.autoClose')}
                                sx={{ color: labelColor }}
                            />
                        </>
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
                <ServerInfo frontendVersion='0.0.1.' version={serverData.version} modules={serverData.modules || []}></ServerInfo>
            </Box >
        </>
    );
};

export default AppSettings;