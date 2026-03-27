import React from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, Typography } from '@mui/material';

import { t } from 'i18next';
import { YesNoWindowProps } from '..';

const YesNoWindow: React.FC<YesNoWindowProps> = ({
    message,
    yesMethod,
    noMethod,
    cancelMethod,
    open,
    onClose,
}) => {
    const handleYes = () => {
        yesMethod?.();
        onClose();
    };

    const handleNo = () => {
        noMethod?.();
        onClose();
    };

    const handleCancel = () => {
        cancelMethod?.();
        onClose();
    };

    return (
        <Dialog open={open} onClose={onClose}>
            <DialogTitle>{t('window.info')}</DialogTitle>
            <DialogContent>
                <Typography>{t(message)}</Typography>
            </DialogContent>
            <DialogActions>
                <Button onClick={handleYes} color="primary" variant="contained" disabled={!yesMethod}>
                    {t('window.yes')}
                </Button>
                <Button onClick={handleNo} color="secondary" variant="outlined" disabled={!noMethod}>
                    {t('window.no')}
                </Button>
                {cancelMethod && (
                    <Button onClick={handleCancel} color="inherit" variant="text">
                        {t('window.cancel')}
                    </Button>
                )}
            </DialogActions>
        </Dialog>
    );
};

export default YesNoWindow;