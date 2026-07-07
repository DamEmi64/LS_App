import React from 'react';
import { Select, MenuItem, InputLabel, FormControl, Box, useTheme } from '@mui/material';
import { changeLanguage } from 'i18next';
import { useTranslation } from 'react-i18next';
import useLocalStorage from 'react-use-localstorage';
import { useAppTheme } from '@/shared/context/theme';
import { AppThemeName } from '@/app/themes';

const languages = [
    { code: 'en', label: 'English' },
    { code: 'pl', label: 'Polski' },
    { code: 'fr', label: 'French' },
    { code: 'de', label: 'German' }
    // Add more languages as needed
];

const GeneralSettings: React.FC = () => {
    const { t, i18n } = useTranslation();
    const [language, setLanguage] = useLocalStorage('lang', i18n.language || 'en');
    const { themeName, setThemeName, themes } = useAppTheme();
    const theme = useTheme();
    const labelColor = theme.palette.text.primary;

    const setTranslation = (lang: string) => {
        changeLanguage(lang);
        setLanguage(lang);
    };

    const setTheme = (nextThemeName: AppThemeName) => {
        setThemeName(nextThemeName);
    }

    return (
        <>
            <Box display="flex" flexDirection="column" gap={2}>
                <FormControl fullWidth>
                    <InputLabel id="theme-select-label" sx={{ color: labelColor }}>
                        {t('settings.theme')}
                    </InputLabel>
                    <Select
                        labelId="theme-select-label"
                        value={themeName}
                        label={t('settings.theme')}
                        onChange={e => setTheme(e.target.value as AppThemeName)}
                        sx={{ color: labelColor }}
                    >
                        {themes.map(theme => (
                            <MenuItem key={theme.name} value={theme.name}>
                                {t(theme.labelKey)}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>
                <FormControl fullWidth>
                    <InputLabel id="settings-language-select-label" sx={{ color: labelColor }}>
                        {t('settings.language')}
                    </InputLabel>
                    <Select
                        labelId="settings-language-select-label"
                        value={language}
                        label={t('settings.language')}
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
            </Box >
        </>
    );
};

export default GeneralSettings;
