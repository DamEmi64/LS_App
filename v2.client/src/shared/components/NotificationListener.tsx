// NotificationListener.js
import React, { useEffect } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { toast, ToastContainer, ToastPosition } from 'react-toastify';
import { ApiConnectContextType, useApiConnect } from '@/shared/context/apiConnect';
import useLocalStorage from 'react-use-localstorage';
import { getNotify } from '@/lib/notifyProvider';

export const NotificationListener = () => {
    var api = useApiConnect();

    useEffect(() => {
        const connection = new HubConnectionBuilder()
            .withUrl(api.getUrl('notify'), {
                withCredentials: true
            }) // Adjust port if needed
            .withAutomaticReconnect()
            .build();

        connection.start()
            .then(() => {
                notify('success', 'Connected to SignalR hub');
                console.log('Connected to SignalR hub');
            })
            .catch(err => notify('error', 'Failed to connect to SignalR hub'));

        connection.on('ReceiveNotification', (type, messageId, args) => {
            const message = getNotify(messageId, args);

            notify(type, message);
        });

        return () => {
            connection.stop();
        };
    }, []);

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