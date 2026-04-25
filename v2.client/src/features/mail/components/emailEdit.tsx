
import {
    Grid,
    TextField,
    Box,
    Button,
    useTheme,
} from "@mui/material";
import { useTranslation } from "react-i18next";
import { useForm, Controller } from "react-hook-form";
import Editor from "./editor";
import  RecipientInput from "@/features/mail/components/recipientInput";
import { useState } from "react";
import { Email } from "../types";

interface EmailEditProps {
    email: Email;
    readonly?: boolean;
    onSave: (email: Email) => void;
}

export const EmailEdit: React.FC<EmailEditProps> = ({ email, onSave, readonly }) => {
    const { t } = useTranslation();
    const theme = useTheme();
    const textColor =
        theme.palette.mode === "dark"
            ? theme.palette.text.primary
            : theme.palette.text.secondary;

    const {
        control,
        handleSubmit,
        formState: { errors },
    } = useForm<Email>({
        defaultValues: email,
    });


    const [recipients, setRecipients] = useState([email.recipient || ""]);


    const extractEmail = (input) => {
        if (!input) return "";

        // Match email inside <>
        const match = input.match(/<([^<>]+@[^<>]+)>/);
        if (match) return match[1].trim();

        // If no <>, check if the string itself is a valid email
        if (/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(input.trim())) {
            return input.trim();
        }

        return "";
    }

    const onSubmit = (data: Email) => {
        const recipientStr = recipients.map(x => extractEmail(x)).join(';');
        data.recipient = recipientStr;
        onSave(data);
    };

    return (
        <form onSubmit={handleSubmit(onSubmit)}>
            <Grid container spacing={2} alignItems="flex-start">
                {/* Sender (readonly) */}
                <Grid size={{ xs: 12 }}>
                    <Controller
                        name="sender"
                        control={control}
                        render={({ field }) => (
                            <TextField
                                {...field}
                                label={t("communication.email.from")}
                                fullWidth
                                margin="dense"
                                variant="outlined"
                                InputProps={{
                                    style: { color: textColor },
                                    readOnly: true,
                                }}
                            />
                        )}
                    />
                </Grid>

                {/* Recipient */}
                <Grid size={{ xs: 12 }}>
                    <RecipientInput suggestions={[]} value={recipients} onChange={(rs) => setRecipients(rs)} label={t("communication.email.to")} />
                </Grid>

                {/* Subject */}
                <Grid size={{ xs: 12 }}>
                    <Controller
                        name="subject"
                        control={control}
                        rules={{ required: t("validation.required") as string }}
                        render={({ field }) => (
                            <TextField
                                {...field}
                                label={t("communication.email.subject")}
                                fullWidth
                                margin="dense"
                                variant="outlined"
                                error={!!errors.subject}
                                helperText={errors.subject?.message}
                                InputProps={{
                                    style: { color: textColor },
                                    readOnly: readonly,
                                }}
                            />
                        )}
                    />
                </Grid>

                {/* Body Editor */}
                <Grid size={{ xs: 12 }}>
                    <Controller
                        name="body"
                        control={control}
                        render={({ field }) => (
                            <Editor
                                initData={field.value || ""}
                                readonly={readonly}
                                onChange={field.onChange}
                            />
                        )}
                    />
                </Grid>

                {/* Save Button */}
                {!readonly && (
                    <Grid size={{ xs: 12 }}>
                        <Box sx={{ display: "flex", gap: 2, mt: 3 }}>
                            <Button type="submit" variant="contained" color="primary">
                                {t("opt.save")}
                            </Button>
                        </Box>
                    </Grid>
                )}
            </Grid>
        </form>
    );
};