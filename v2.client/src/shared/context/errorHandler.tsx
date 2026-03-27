import React, { createContext, useContext, useState, ReactNode, useEffect } from 'react';
import { notify } from '@/shared/components/NotificationListener';
import { AxiosError } from 'axios';

type ErrorHandlerContextType = {
    setError: (error: Error | string) => void;
};

const ErrorHandlerContext = createContext<ErrorHandlerContextType | undefined>(undefined);

export const useErrorHandler = () => {
    const context = useContext(ErrorHandlerContext);
    if (!context) {
        throw new Error('useErrorHandler must be used within an ErrorHandlerProvider');
    }
    return context;
};

type ErrorHandlerProviderProps = {
    children: ReactNode;
};

export const ErrorHandlerProvider: React.FC<ErrorHandlerProviderProps> = ({ children }) => {
    const [error, setError] = useState<Error | string | null>(null);

    useEffect(() => {
        const handleGlobalError = (event: ErrorEvent) => {
            setError(event.error ?? event.message);
        };

        const handlePromiseRejection = (event: PromiseRejectionEvent) => {
            setError(event.reason instanceof Error ? event.reason : String(event.reason));
        };

        window.addEventListener('error', handleGlobalError);
        window.addEventListener('unhandledrejection', handlePromiseRejection);

        return () => {
            window.removeEventListener('error', handleGlobalError);
            window.removeEventListener('unhandledrejection', handlePromiseRejection);
        };
    }, []);

    useEffect(() => {
        if (!error) return;

        if (error instanceof AxiosError) {
            return;
        }


        // Display the notification
        if (error instanceof Error) {
            notify('error', error.message);
        }

        console.error('Captured error:', error); // optional logging
        setError(null);
    }, [error]);

    return (
        <ErrorHandlerContext.Provider value={{ setError }}>
            {children}
        </ErrorHandlerContext.Provider>
    );
};