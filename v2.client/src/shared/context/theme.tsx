import { createContext, ReactNode, useContext, useMemo, useState } from 'react';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import useLocalStorage from 'react-use-localstorage';
import { appThemes, AppThemeName, defaultThemeName, getAppTheme, withThemeHelpers } from '@/app/themes';

type AppThemeContextType = {
    themeName: AppThemeName;
    setThemeName: (themeName: AppThemeName) => void;
};

const AppThemeContext = createContext<AppThemeContextType | null>(null);

export function AppThemeProvider({ children }: { children: ReactNode }) {
    const [storedThemeName, setStoredThemeName] = useLocalStorage('theme', defaultThemeName);
    const [themeName, setThemeNameState] = useState<AppThemeName>(getAppTheme(storedThemeName).name);

    const selectedTheme = getAppTheme(themeName);
    const theme = useMemo(
        () => withThemeHelpers(createTheme(selectedTheme.options)),
        [selectedTheme]
    );

    const setThemeName = (nextThemeName: AppThemeName) => {
        const nextTheme = getAppTheme(nextThemeName);

        setThemeNameState(nextTheme.name);
        setStoredThemeName(nextTheme.name);
    };

    return (
        <AppThemeContext.Provider value={{ themeName, setThemeName }}>
            <ThemeProvider theme={theme}>
                {children}
            </ThemeProvider>
        </AppThemeContext.Provider>
    );
}

export function useAppTheme() {
    const ctx = useContext(AppThemeContext);

    if (!ctx) {
        throw new Error('useAppTheme must be used within AppThemeProvider');
    }

    return {
        ...ctx,
        themes: appThemes,
    };
}
