import { Box, Typography, TextField, Button } from "@mui/material";
import { t } from "i18next";
import { Controller, useForm } from "react-hook-form";
import { FileEditFormData, FileV2 } from "../types";
import { useEffect, useState } from "react";
import FileUploader from "@/shared/components/fileUploader";


interface FileWrapperProps {
    file: FileV2 | null;
    onSubmit: (file: FileEditFormData) => void;
    readonly?: boolean
}

export default function FileWrapper({ file, onSubmit, readonly }: FileWrapperProps) {

    const [content, setContent] = useState<File | null>();

    const {
        control: editControl,
        handleSubmit: handleEditSubmit,
        reset: resetEdit,
        formState: { errors: editErrors },
    } = useForm<FileEditFormData>({
        defaultValues: {
            title: "",
            description: "",
        },
    });

    useEffect(() => {
        if (!file) {
            resetEdit();
            return;
        }
        resetEdit({
            title: file.title || "",
            description: file.description || "",
        });
    }, [file, resetEdit]);

    if (!file) return null;

    const handleSubmit = (data: FileEditFormData) => {
        data.File = content;
        onSubmit(data);
    }

    return (
        <form onSubmit={handleEditSubmit(handleSubmit)}>
            <Box sx={{ mb: 3 }}>
                <Typography variant="h6" sx={{ mb: 2 }}>
                    {t('window.info')}
                </Typography>

                {/* Title field */}
                <Controller
                    name="title"
                    control={editControl}
                    rules={{ required: t('validation.required') as string }}
                    render={({ field }) => (
                        <TextField
                            {...field}
                            label={t('files.name')}
                            fullWidth
                            margin="dense"
                            error={!!editErrors.title}
                            helperText={editErrors.title?.message}
                            disabled={readonly}
                        />
                    )}
                />

                {/* Description field */}
                <Controller
                    name="description"
                    control={editControl}
                    render={({ field }) => (
                        <TextField
                            {...field}
                            label={t('rpg.story.description')}
                            multiline
                            minRows={3}
                            maxRows={8}
                            fullWidth
                            margin="dense"
                            error={!!editErrors.description}
                            helperText={editErrors.description?.message}
                            disabled={readonly}
                        />
                    )}
                />

                {!readonly && (
                    <>
                        <FileUploader onUpload={f => setContent(f)} />
                        <Box sx={{ display: "flex", gap: 2, mt: 2 }}>
                            <Button
                                type="submit"
                                variant="contained"
                                color="primary"
                           >
                                {t('opt.save')}
                            </Button>
                        </Box>
                    </>
                )}
            </Box>
        </form>
    )
}