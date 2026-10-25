import React, { useState } from "react";
import { Alert, Button, Dialog, DialogActions, DialogContent, DialogTitle, TextField, Typography } from "@mui/material";
import { useTranslation } from "react-i18next";
import { isAndroidApp } from '@/shared/platform';

type EndMeetingDialogProps = {
    open: boolean;
    onClose: () => void;
    onConfirm: (summary: string) => Promise<void>;
    initialSummary?: string;
    generatingSummary?: boolean;
    transcript?: string;
    transcriptionError?: string;
    generationStatus?: string;
    downloadingModel?: boolean;
};

const EndMeetingDialog = ({ open, onClose, onConfirm, initialSummary = "", generatingSummary = false, transcript = "", transcriptionError = "", generationStatus = "", downloadingModel = false }: EndMeetingDialogProps) => {
    const { t } = useTranslation();
    const [summary, setSummary] = useState(initialSummary);
    const [submitting, setSubmitting] = useState(false);
    React.useEffect(() => {
        if (open) setSummary(initialSummary);
    }, [initialSummary, open]);

    const handleConfirm = async () => {
        setSubmitting(true);

        try {
            await onConfirm(summary);
            setSummary("");
            onClose();
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <Dialog open={open} onClose={submitting || generatingSummary ? undefined : onClose} fullWidth maxWidth="sm">
            <DialogTitle>{t("rpg.chapter.end")}</DialogTitle>
            <DialogContent>
                {isAndroidApp && <>
                    {downloadingModel && <Alert severity="info" sx={{ mb: 2 }}>{t("rpg.meeting.modelDownloadInfo", "First use downloads about 104 MB of speech model files. Your recording stays on this device.")}</Alert>}
                    {transcriptionError && <Alert severity="warning" sx={{ mb: 2 }}>{transcriptionError}</Alert>}
                    {transcript && <>
                        <Typography variant="subtitle2" sx={{ mt: 1, mb: 0.5 }}>{t("rpg.meeting.transcript", "Transcript (saved on this device)")}</Typography>
                        <TextField fullWidth multiline minRows={3} maxRows={8} value={transcript} inputProps={{ readOnly: true }} />
                    </>}
                </>}

                <TextField
                    autoFocus
                    fullWidth
                    multiline
                    minRows={5}
                    label={t("rpg.story.summary")}
                    value={summary}
                    onChange={(event) => setSummary(event.target.value)}
                    disabled={submitting || generatingSummary}
                    sx={{ mt: 1 }}
                />
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose} disabled={submitting || generatingSummary}>
                    {t("opt.cancel")}
                </Button>
                <Button variant="contained" onClick={handleConfirm} disabled={submitting || generatingSummary}>
                    {generatingSummary ? (generationStatus || t("rpg.meeting.generatingSummary", "Generating summary…")) : t("rpg.chapter.end")}
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default EndMeetingDialog;
