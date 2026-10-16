import { useState } from 'react';
import { Button, Dialog, DialogActions, DialogContent, DialogTitle, FormControlLabel, Radio, RadioGroup } from '@mui/material';
import { useTranslation } from 'react-i18next';

type StartMeetingDialogProps = {
    open: boolean;
    onClose: () => void;
    onConfirm: (record: boolean) => Promise<void>;
};

export default function StartMeetingDialog({ open, onClose, onConfirm }: StartMeetingDialogProps) {
    const { t } = useTranslation();
    const [record, setRecord] = useState('no');
    const [starting, setStarting] = useState(false);

    const start = async () => {
        setStarting(true);
        try {
            await onConfirm(record === 'yes');
            onClose();
        } finally {
            setStarting(false);
        }
    };

    return (
        <Dialog open={open} onClose={starting ? undefined : onClose} fullWidth maxWidth="xs">
            <DialogTitle>{t('rpg.meeting.startTitle', 'Start RPG meeting')}</DialogTitle>
            <DialogContent>
                <RadioGroup value={record} onChange={event => setRecord(event.target.value)}>
                    <FormControlLabel value="no" control={<Radio />} label={t('rpg.meeting.withoutRecording', 'Start without recording')} />
                    <FormControlLabel value="yes" control={<Radio />} label={t('rpg.meeting.withRecording', 'Record on this device')} />
                </RadioGroup>
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose} disabled={starting}>{t('opt.cancel')}</Button>
                <Button variant="contained" onClick={start} disabled={starting}>
                    {t('rpg.chapter.start')}
                </Button>
            </DialogActions>
        </Dialog>
    );
}
