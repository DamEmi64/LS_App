import React, { useEffect, useState } from 'react';
import { Avatar, Badge, Box, Button, Divider, IconButton, List, ListItemButton, Paper, Popover, Stack, Typography } from '@mui/material';
import NotificationsNoneIcon from '@mui/icons-material/NotificationsNone';
import { toast, ToastContainer, ToastPosition } from 'react-toastify';
import { useNotificationCenter } from 'react-toastify/addons/use-notification-center';
import { getNotify } from '@/lib/notifyProvider';
import { useSignalR } from '../hooks/use-signalR';
import { appStorage } from '@/shared/storage/appStorage';
import { useTranslation } from 'react-i18next';

type NotificationType = 'success' | 'warning' | 'error' | 'info' | 'process' | 'process-error' | string;
type NotificationData = { title?: string; text?: string };

const notificationStyles: Record<string,any> = {
    success: { border: '#2e7d32', background: 'success.light', foreground: 'success.dark' },
    warning: { border: '#ed6c02', background: 'warning.light', foreground: 'warning.dark' },
    error: { border: '#d32f2f', background: 'error.light', foreground: 'error.dark' },
    'process-error': { border: '#d32f2f', background: 'error.light', foreground: 'error.dark' },
    info: { border: '#0288d1', background: 'info.light', foreground: 'info.dark' },
    process: { border: '#0288d1', background: 'info.light', foreground: 'info.dark' },
    default: { border: 'divider', background: 'background.paper', foreground: 'text.primary' },
};

export const NotificationListener = () => {
    const { on } = useSignalR('notify', () => notify('info', 'Connected to notification service'));

    const handleNotification = React.useCallback((type: NotificationType, messageId: string, args: any) => {
        notify(type, getNotify(messageId, args));
    }, []);

    useEffect(() => {
        on('ReceiveNotification', handleNotification);
    }, [on, handleNotification]);

    const position = (appStorage.get('toastPosition') || 'bottom-right') as ToastPosition;
    const autoCloseSetting = Number(appStorage.get('toastAutoClose') || 3000);

    return <ToastContainer stacked position={position} autoClose={Number.isFinite(autoCloseSetting) ? autoCloseSetting : 3000} />;
};

export const NotificationCenterButton = () => {
    const { t } = useTranslation();
    const { notifications, unreadCount, markAsRead, markAllAsRead, clear } = useNotificationCenter<NotificationData>();
    const [isOpen, setIsOpen] = useState(false);
    const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);

    return (
        <>
                <IconButton
                    aria-label={`${t('notify.center.title')}${unreadCount ? `, ${t('notify.center.unreadCount', { count: unreadCount })}` : ''}`}
                    size="small"
                    sx={{ ml: 2 }}
                    aria-controls={open ? 'account-menu' : undefined}
                    aria-haspopup="true"
                    aria-expanded={open ? 'true' : undefined}
                    onClick={event => {
                        setAnchorEl(event.currentTarget);
                        setIsOpen(open => !open);
                    }}
                >
                    <Badge badgeContent={unreadCount} color="error" max={99}>
                            <Avatar sx={{ width: 32, height: 32 }}>
                                <NotificationsNoneIcon />
                            </Avatar>
                        </Badge>
                </IconButton>

                <Popover
                    open={isOpen}
                    anchorEl={anchorEl}
                    onClose={() => setIsOpen(false)}
                    anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
                    transformOrigin={{ vertical: 'top', horizontal: 'right' }}
                    slotProps={{ paper: { sx: { mt: 1, width: { xs: 'calc(100vw - 32px)', sm: 360 }, maxHeight: '75vh', overflow: 'hidden' } } }}
                >
                    <Paper aria-label={t('notify.center.title')} elevation={0}>
                        <Stack direction="row" alignItems="center" justifyContent="space-between" sx={{ px: 2, py: 1.5 }}>
                            <Box>
                                <Typography variant="subtitle1" fontWeight={600}>{t('notify.center.title')}</Typography>
                                <Typography variant="caption" color="text.secondary">{t('notify.center.unreadCount', { count: unreadCount })}</Typography>
                            </Box>
                            <Stack direction="row" spacing={0.5}>
                                <Button size="small" onClick={() => markAllAsRead()}>{t('notify.center.markAllRead')}</Button>
                                <Button size="small" color="inherit" onClick={clear}>{t('notify.center.clear')}</Button>
                            </Stack>
                        </Stack>
                        <Divider />
                        <Box sx={{ maxHeight: 'calc(75vh - 72px)', overflowY: 'auto', p: 1 }}>
                            {notifications.length === 0 ? (
                                <Typography color="text.secondary" variant="body2" align="center" sx={{ py: 5 }}>{t('notify.center.caughtUp')}</Typography>
                            ) : (
                                <List disablePadding>
                                    {notifications.map(item => {
                                        const type = item.data?.title || item.type || 'default';
                                        const color = notificationStyles[type] || notificationStyles.default;
                                        const message = item.data?.text || (typeof item.content === 'string' ? item.content : t('notify.center.notification'));
                                        return (
                                            <ListItemButton
                                                key={item.id}
                                                onClick={() => markAsRead(item.id)}
                                                sx={{
                                                    display: 'block', mb: 1, borderLeft: 4, borderColor: color.border,
                                                    borderRadius: 1, bgcolor: color.background, color: color.foreground,
                                                    opacity: item.read ? 0.72 : 1,
                                                }}
                                            >
                                                <Stack direction="row" alignItems="center" justifyContent="space-between" spacing={1}>
                                                    <Typography variant="overline" fontWeight={700} lineHeight={1.5}>{t(`notify.center.types.${type}`, type)}</Typography>
                                                    {!item.read && <Box aria-label={t('notify.center.unread')} sx={{ width: 8, height: 8, borderRadius: '50%', bgcolor: 'currentColor' }} />}
                                                </Stack>
                                                <Typography variant="body2" sx={{ overflowWrap: 'anywhere' }}>{message}</Typography>
                                                <Typography component="time" dateTime={new Date(item.createdAt).toISOString()} variant="caption" sx={{ display: 'block', mt: 1, opacity: 0.72 }}>
                                                    {new Date(item.createdAt).toLocaleString()}
                                                </Typography>
                                            </ListItemButton>
                                        );
                                    })}
                                </List>
                            )}
                        </Box>
                    </Paper>
                </Popover>
        </>
    );
};

export const notify = (type: NotificationType, message: string) => {
    const active = appStorage.get('toastActive') !== 'false';
    const showProcess = appStorage.get('toastProcess') === 'true';
    const showProcessErrors = appStorage.get('toastProcessError') === 'true';
    if (!active) return;

    const position = (appStorage.get('toastPosition') || 'bottom-right') as ToastPosition;
    const autoCloseSetting = Number(appStorage.get('toastAutoClose') || 3000);
    const options = {
        position,
        autoClose: Number.isFinite(autoCloseSetting) ? autoCloseSetting : 3000,
        data: { title: type, text: message },
    };

    switch (type) {
        case 'success': toast.success(message, options); break;
        case 'error':
        case 'process-error':
            if (type !== 'process-error' || showProcessErrors) toast.error(message, options);
            break;
        case 'warning': toast.warn(message, options); break;
        case 'info': toast.info(message, options); break;
        case 'process': if (showProcess && !showProcessErrors) toast.info(message, options); break;
        default: toast(message, options);
    }
};
