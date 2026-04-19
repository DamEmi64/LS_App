import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "@/layout/layout";

import Index from "@/features/system/pages/Index";
import IndexImg from "@/assets/index.jpg";

import Files from "@/features/files/pages/Files";
import FilesImg from "@/assets/files.jpg";

import Processes from "@/features/system/pages/Processes";
import ProcessesImg from "@/assets/settings.jpg";

import Templates from "@/features/mail/pages/templates";
import TemplateImg from "@/assets/template.jpg";

import Emails from "@/features/mail/pages/emails";
import EmailImg from "@/assets/emails.jpg";

import RPG from "@/features/rpg/pages/Page";
import RPGImg from "@/assets/rpg.jpg";
import PlayerPage from "@/features/rpg/pages/PlayerPage";

import Automations from "@/features/automation/pages/List";
import AutomationsImg from "@/assets/settings.jpg";


import NotFound from "@/features/system/pages/NotFound";

import SlideRoutes from 'react-slide-routes';
import "@/shared/localization/i18n"; // Ensure i18n is initialized
import { AuthProvider } from "@/features/auth/context/authProvider";
import { ModalProvider } from "@/shared/context/modal";
import { ApiConnect } from "@/shared/context/apiConnect";
import { ThemeProvider, createTheme } from '@mui/material/styles';
import { ErrorHandlerProvider } from "@/shared/context/errorHandler";

import { NotificationListener } from "@/shared/components/NotificationListener";
import { NavbarItemProps } from "@/shared";
import PlayerViewPage from "@/features/rpg/pages/PlayerViewPage";
import { ProgressFlow } from "@/features/rpg/components/flow/ProgressFlow";

const queryClient = new QueryClient();

const theme = createTheme({
    colorSchemes: {
        dark: true,
    },
});

const menu: NavbarItemProps[] = [
    { label: 'home', href: '/', submenu: [] },
    {
        label: 'communication', href: '', submenu: [
            {
                label: 'emails', href: '/Emails',
                submenu: []
            },
            {
                label: 'templates', href: '/Templates',
                submenu: []
            },
        ], permissions: ['communication']
    },
    {
        label: 'rpg_sessions', href: '', submenu: [
            {
                label: 'rpg_sessions', href: '/rpg',
                submenu: [],
                permissions: ['rpg']
            },
            {
                label: 'draft', href: '/rpg/drafts',
                submenu: [],
                permissions: ['rpg_draft']
            },
        ], permissions: ['rpg']
    },
    { label: 'automations', href: '/automations', submenu: [], permissions: ['automation'] },
];

const App = () => (
    <ThemeProvider theme={theme}>
        <ErrorHandlerProvider>
            <ApiConnect>
                <ModalProvider>
                    <QueryClientProvider client={queryClient}>
                        <AuthProvider>
                            <BrowserRouter>
                                <SlideRoutes>
                                    <Route path="/" element={<Layout content={Index} image={IndexImg} title={'menu.home'} menu={menu} />} />
                                    <Route path="/test" element={<Layout content={ProgressFlow} image={IndexImg} title={'menu.home'} menu={menu} allowAnonymous />} />
                                    <Route path="/processes" element={<Layout content={Processes} image={ProcessesImg} title={'menu.processes'} permissions={['processes']} menu={menu} />} />
                                    <Route path="/emails" element={<Layout content={Emails} image={EmailImg} title={'menu.emails'} permissions={['communication']} menu={menu} />} />
                                    <Route path="/templates" element={<Layout content={Templates} image={TemplateImg} title={'menu.templates'} permissions={['communication']} menu={menu} />} />
                                    <Route path="/rpg/playerData" element={<Layout content={PlayerPage} image={RPGImg} title={'menu.rpg_sessions'} permissions={['rpg']} menu={menu} />} />
                                    <Route path="/rpg/playerView" element={<Layout content={PlayerViewPage} image={RPGImg} title={'menu.rpg_sessions'} menu={menu} allowAnonymous />} />
                                    <Route path="/rpg" element={<Layout content={o => <RPG draft={false} />} image={RPGImg} title={'menu.rpg_sessions'} permissions={['rpg']} menu={menu} />} />
                                    <Route path="/rpg/drafts" element={<Layout content={o => <RPG draft={true} />} image={RPGImg} title={'menu.rpg_sessions'} permissions={['rpg']} menu={menu} />} />
                                    <Route path="/automations" element={<Layout content={Automations} image={AutomationsImg} title={'menu.automations'} permissions={['automation']} menu={menu} />} />
                                    {/* ADD ALL CUSTOM ROUTES ABOVE THE CATCH-ALL "*" ROUTE */}
                                    <Route path="*" element={<Layout content={NotFound} image={IndexImg} title={'404'} menu={menu} />} />
                                </SlideRoutes>
                            </BrowserRouter>
                            <NotificationListener />
                        </AuthProvider>
                    </QueryClientProvider>
                </ModalProvider>
            </ApiConnect>
        </ErrorHandlerProvider>
    </ThemeProvider>
);

export default App;