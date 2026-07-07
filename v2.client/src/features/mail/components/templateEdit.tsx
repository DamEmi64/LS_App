import { Template } from "@/features/mail";
import { Grid, Box, Button, TextField, useTheme } from "@mui/material";
import { useTranslation } from "react-i18next";
import { useForm, Controller } from "react-hook-form";
import Editor from "./editor";
import TemplateRules from "./templateRules";

interface TemplateEditProps {
    template: Template;
    readonly?: boolean;
    onSave: (template: Template) => void;
    style?: React.CSSProperties;
}

export const TemplateEdit: React.FC<TemplateEditProps> = ({ template, onSave, readonly, style }) => {
    const { t } = useTranslation();
    const theme = useTheme();
    const textColor = theme.palette.text.primary;

    const {
        control,
        handleSubmit,
        formState: { errors },
    } = useForm<Template>({
        defaultValues: template || {} as Template,
    });

    const onSubmit = (data: Template) => {
        if (onSave) onSave(data);
    };

    return (
        <form onSubmit={handleSubmit(onSubmit)} style={style}>
            <Grid container spacing={2} alignItems="flex-start">
                {/* Template rules */}
                <Grid size={{ xs: 12 }}>
                    <TemplateRules />
                </Grid>

                {/* Subject (required) */}
                <Grid size={{ xs: 12 }}>
                    <Controller
                        name="subject"
                        control={control}
                        rules={{ required: t("validation.required") as string }}
                        render={({ field }) => (
                            <TextField
                                {...field}
                                label={t("communication.template.subject")}
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

                {/* Save button */}
                {!readonly && (
                    <Grid size={{ xs: 12 }}>
                        <Box sx={{ display: "flex", gap: 2, mt: 3 }}>
                            <Button type="submit" variant="contained" color="primary">
                                {t("opt.save")}
                            </Button>
                        </Box>
                    </Grid>
                )}

                {/* Body editor */}
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
            </Grid>
        </form>
    );
};
