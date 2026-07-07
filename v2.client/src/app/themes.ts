import { alpha, ThemeOptions } from '@mui/material/styles';

export type AppThemeName = 'light' | 'dark' | 'ocean' | 'forest' | 'rose';

export type AppThemeDefinition = {
    name: AppThemeName;
    labelKey: string;
    labelColor: string;
    options: ThemeOptions;
};

export const defaultThemeName: AppThemeName = 'dark';

export const appThemes: AppThemeDefinition[] = [
    {
        name: 'light',
        labelKey: 'settings.themes.light',
        labelColor: '#111827',
        options: {
            palette: {
                mode: 'light',
                primary: { main: '#2563eb' },
                secondary: { main: '#7c3aed' },
                background: { default: '#f8fafc', paper: '#ffffff' },
                text: { primary: '#111827', secondary: '#475569' },
            },
        },
    },
    {
        name: 'dark',
        labelKey: 'settings.themes.dark',
        labelColor: '#f8fafc',
        options: {
            palette: {
                mode: 'dark',
                primary: { main: '#60a5fa' },
                secondary: { main: '#c084fc' },
                background: { default: '#0f172a', paper: '#111827' },
                text: { primary: '#f8fafc', secondary: '#cbd5e1' },
            },
        },
    },
    {
        name: 'ocean',
        labelKey: 'settings.themes.ocean',
        labelColor: '#e0f2fe',
        options: {
            palette: {
                mode: 'dark',
                primary: { main: '#38bdf8' },
                secondary: { main: '#2dd4bf' },
                background: { default: '#082f49', paper: '#0c4a6e' },
                text: { primary: '#e0f2fe', secondary: '#bae6fd' },
            },
        },
    },
    {
        name: 'forest',
        labelKey: 'settings.themes.forest',
        labelColor: '#ecfdf5',
        options: {
            palette: {
                mode: 'dark',
                primary: { main: '#34d399' },
                secondary: { main: '#fbbf24' },
                background: { default: '#052e16', paper: '#064e3b' },
                text: { primary: '#ecfdf5', secondary: '#bbf7d0' },
            },
        },
    },
    {
        name: 'rose',
        labelKey: 'settings.themes.rose',
        labelColor: '#fff1f2',
        options: {
            palette: {
                mode: 'dark',
                primary: { main: '#fb7185' },
                secondary: { main: '#facc15' },
                background: { default: '#4c0519', paper: '#881337' },
                text: { primary: '#fff1f2', secondary: '#fecdd3' },
            },
        },
    },
];

export const getAppTheme = (themeName: string | null | undefined) =>
    appThemes.find(theme => theme.name === themeName) ?? appThemes.find(theme => theme.name === defaultThemeName)!;

export const withThemeHelpers = <TTheme extends object>(theme: TTheme) =>
    Object.assign(theme, { alpha });
