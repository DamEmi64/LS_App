// NotificationListener.js
import React, { useEffect } from 'react';
import { toast, ToastContainer, ToastPosition } from 'react-toastify';
import { getNotify } from '@/lib/notifyProvider';
import { useSignalR } from '../hooks/use-signalR';

export const NotificationListener = () => {
    const { on } = useSignalR('notify', () =>
        notify('info', 'Connected to notification service')
    );

    const handleNotification = React.useCallback((type, messageId, args) => {
        const message = getNotify(messageId, args);
        notify(type, message);
    }, []);

    useEffect(() => {
        on('ReceiveNotification', handleNotification);
    }, [on, handleNotification]);

    return <ToastContainer position="top-right" autoClose={3000} />;
};

export const notify = (type, message) => {
    const position = localStorage['toastPosition'] || "bottom-right";
    const autoClose = localStorage['toastAutoClose'] || '3000';
    const active = localStorage['toastActive'] || 'true';
    const process = localStorage['toastProcess'] || 'false';
    const processError = localStorage['toastProcessError'] || 'false';

    if (active) {
        switch (type) {
            case 'success':
                toast.success(message, {
                    position: position as ToastPosition,
                    autoClose: autoClose as unknown as number
                });
                break;
            case 'error':
                toast.error(message, {
                    position: position as ToastPosition,
                    autoClose: autoClose as unknown as number
                });
                break;
            case 'info':
                toast.info(message, {
                    position: position as ToastPosition,
                    autoClose: autoClose as unknown as number
                });
                break;
            case 'warning':
                toast.warn(message, {
                    position: position as ToastPosition,
                    autoClose: autoClose as unknown as number
                });
                break;
            case 'process':
                if (process && !processError) {
                    toast.info(message, {
                        position: position as ToastPosition,
                        autoClose: autoClose as unknown as number
                    });
                }
                break;
            case 'process-error':
                if (process) {
                    toast.error(message, {
                        position: position as ToastPosition,
                        autoClose: autoClose as unknown as number
                    });
                }
                break;
            default:
                toast(message);
        }
    }
};