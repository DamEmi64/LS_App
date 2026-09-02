import { App } from '@capacitor/app';
import { useEffect, useRef } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { useModal } from '@/shared/context/modal';
import { isAndroidApp } from '@/shared/platform';

export function AndroidBackButtonHandler() {
    const modal = useModal();
    const navigate = useNavigate();
    const location = useLocation();
    const hasNavigatedWithinApp = useRef(false);
    const currentPath = useRef(location.pathname);

    useEffect(() => {
        if (currentPath.current !== location.pathname) {
            hasNavigatedWithinApp.current = true;
            currentPath.current = location.pathname;
        }
    }, [location.pathname]);

    useEffect(() => {
        if (!isAndroidApp) return;

        let removeListener: (() => void) | undefined;

        void App.addListener('backButton', () => {
            if (modal.closeTopMostModal()) return;

            const drawerBackEvent = new Event('app-back', { cancelable: true });
            window.dispatchEvent(drawerBackEvent);
            if (drawerBackEvent.defaultPrevented) return;

            if (hasNavigatedWithinApp.current) {
                navigate(-1);
            } else if (location.pathname !== '/') {
                navigate('/');
            }
            // At the root, intentionally do nothing: Android Back never exits the app.
        }).then(listener => {
            removeListener = () => listener.remove();
        });

        return () => removeListener?.();
    }, [location.pathname, modal, navigate]);

    return null;
}
