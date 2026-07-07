import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "@/layout/layout";

import Index from "@/features/system/pages/Index";
import IndexImg from "@/assets/index.jpg";

import Files from "@/features/files/pages/Files";
import FilesImg from "@/assets/files.jpg";

import Processes from "@/features/system/pages/Processes";
import ProcessesImg from "@/assets/processes.png";

import Templates from "@/features/mail/pages/templates";
import TemplateImg from "@/assets/template.jpg";

import Emails from "@/features/mail/pages/emails";
import EmailImg from "@/assets/emails.png";

import CommunicationHistory from "@/features/mail/pages/CommunicationRegistry";
import CommunicationHistoryImg from "@/assets/emails.png";

import EventsPage from "@/features/events/pages/allPage";
import MyEventsPage from "@/features/events/pages/myPage";
import EventsImg from "@/assets/events.png";

import RPG from "@/features/rpg/pages/Page";
import RPGImg from "@/assets/rpg.png";
import PlayerPage from "@/features/rpg/pages/PlayerPage";

import Automations from "@/features/automation/pages/List";
import AutomationsImg from "@/assets/settings.png";


import NotFound from "@/features/system/pages/NotFound";

import SlideRoutes from 'react-slide-routes';
import "@/shared/localization/i18n"; // Ensure i18n is initialized
import { AuthProvider } from "@/features/auth/context/authProvider";
import { ModalProvider } from "@/shared/context/modal";
import { ErrorHandlerProvider } from "@/shared/context/errorHandler";
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { NotificationListener } from "@/shared/components/NotificationListener";
import { NavbarItemProps } from "@/shared";
import PlayerViewPage from "@/features/rpg/pages/PlayerViewPage";
import { Configuration } from "@/shared/api/generated";
import { ConfigurationProvider } from "@/shared/context/configuration";
import CommunicationRegistry from "@/features/mail/pages/CommunicationRegistry";
import { AppThemeProvider } from "@/shared/context/theme";

const queryClient = new QueryClient();

const menu: NavbarItemProps[] = [
    { label: 'home', href: '/', submenu: [] },
    { label: 'files', href: '/Files', submenu: [], permissions: ['files'] },
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
            {
                label: 'communicationRegistry', href: '/communicationHistory',
                submenu: [],
                permissions: ['communication-registry']
            },
        ], permissions: ['communication']
    },
    {
        label: 'events', href: '', submenu: [
            {
                label: 'allEvents', href: '/events',
                submenu: [],
                permissions: ['events']
            },
            {
                label: 'myEvents', href: '/events/me',
                submenu: [],
                permissions: ['events']
            },
        ],
        permissions: ['events']
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
                permissions: ['rpg-draft']
            },
        ], permissions: ['rpg']
    },
    { label: 'automations', href: '/automations', submenu: [], permissions: ['automation'] },
];

const App = () => (
    <ConfigurationProvider>
        <LocalizationProvider dateAdapter={AdapterDayjs}>
            <AppThemeProvider>
                <ErrorHandlerProvider>
                    <ModalProvider>
                        <QueryClientProvider client={queryClient}>
                            <AuthProvider>
                                <BrowserRouter>
                                    <SlideRoutes>
                                        <Route path="/" element={<Layout content={Index} image={IndexImg} title={'menu.home'} menu={menu} />} />
                                        <Route path="/processes" element={<Layout content={Processes} image={ProcessesImg} title={'menu.processes'} permissions={['processes']} menu={menu} />} />
                                        <Route path="/files" element={<Layout content={Files} image={FilesImg} title={'menu.files'} permissions={['files']} menu={menu} />} />
                                        <Route path="/emails" element={<Layout content={Emails} image={EmailImg} title={'menu.emails'} permissions={['communication']} menu={menu} />} />
                                        <Route path="/communicationHistory" element={<Layout content={CommunicationRegistry} image={EmailImg} title={'menu.communicationRegistry'} permissions={['communication-registry']} menu={menu} />} />
                                        <Route path="/events" element={<Layout content={EventsPage} image={EventsImg} title={'menu.events'} permissions={['events']} menu={menu} />} />
                                        <Route path="/events/me" element={<Layout content={MyEventsPage} image={EventsImg} title={'menu.myEvents'} permissions={['events']}  menu={menu} />} />
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
                </ErrorHandlerProvider>
            </AppThemeProvider>
        </LocalizationProvider>
    </ConfigurationProvider>


);

export default App;
