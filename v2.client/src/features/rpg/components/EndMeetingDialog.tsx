import { useState } from "react";
import { Button, Dialog, DialogActions, DialogContent, DialogTitle, TextField } from "@mui/material";
import { useTranslation } from "react-i18next";

type EndMeetingDialogProps = {
    open: boolean;
    onClose: () => void;
    onConfirm: (summary: string) => Promise<void>;
};

const EndMeetingDialog = ({ open, onClose, onConfirm }: EndMeetingDialogProps) => {
    const { t } = useTranslation();
    const [summary, setSummary] = useState("");
    const [submitting, setSubmitting] = useState(false);

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
        <Dialog open={open} onClose={submitting ? undefined : onClose} fullWidth maxWidth="sm">
            <DialogTitle>{t("rpg.chapter.end")}</DialogTitle>
            <DialogContent>
                <TextField
                    autoFocus
                    fullWidth
                    multiline
                    minRows={5}
                    label={t("rpg.story.summary")}
                    value={summary}
                    onChange={(event) => setSummary(event.target.value)}
                    disabled={submitting}
                    sx={{ mt: 1 }}
                />
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose} disabled={submitting}>
                    {t("opt.cancel")}
                </Button>
                <Button variant="contained" onClick={handleConfirm} disabled={submitting}>
                    {t("rpg.chapter.end")}
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default EndMeetingDialog;
