import React, { useState } from 'react';
import {
    Box,
    TextField,
    Typography,
    useTheme,
    Grid,
    Button,
} from '@mui/material';
import { t } from 'i18next';
import { useForm, Controller } from 'react-hook-form';
import { Story } from '@/features/rpg';
import FileManager from '@/shared/components/fileManager';
import { call, FileItem } from '@/shared';

type SessionInfoProps = {
    story: Story;
    toSave?: (data: Story) => void;
    edit?: boolean
};

export const StoryEdit: React.FC<SessionInfoProps> = ({ story, toSave, edit }) => {
    const theme = useTheme();
    const textColor = theme.palette.text.primary;

    const {
        control,
        handleSubmit,
        formState: { errors },
    } = useForm<Story>({
        defaultValues: {
            ...story,
        },
    });

    const onSubmit = (data: Story) => {
        if (toSave) toSave(data);
    };

    const [files, setFiles] = useState(story.files);

    const refreshFiles = () => {
        call<Story>(api => api.storiesApi.getById,{id:story.id}).then(s => setFiles(s.files))
    }

    return (
        <form onSubmit={handleSubmit(onSubmit)}>
            <Grid container spacing={2} alignItems="flex-start">
                {/* Left side: Info */}
                <Grid size={{ xs: 12 }}>
                    <Typography variant="h6" sx={{ color: textColor, mb: 2 }}>
                        {t('window.info')}
                    </Typography>

                    {/* Title (required) */}
                    <Controller
                        name="title"
                        control={control}
                        rules={{ required: t('validation.required') as string }}
                        render={({ field }) => (
                            <TextField
                                {...field}
                                label={t('files.name')}
                                fullWidth
                                margin="dense"
                                variant="outlined"
                                error={!!errors.title}
                                helperText={errors.title?.message}
                                InputProps={{ style: { color: textColor } }}
                            />
                        )}
                    />

                    {/* Description (required) */}
                    <Controller
                        name="description"
                        control={control}
                        rules={{ required: t('validation.required') as string }}
                        render={({ field }) => (
                            <TextField
                                {...field}
                                label={t('rpg.story.description')}
                                multiline
                                minRows={4}
                                maxRows={16}
                                fullWidth
                                margin="dense"
                                variant="outlined"
                                error={!!errors.description}
                                helperText={errors.description?.message}
                                InputProps={{ style: { color: textColor } }}
                            />
                        )}
                    />

                    {edit && (
                        <FileManager files={files || []}
                            editMode={true}
                            add={o => call(api => api.storiesApi.createByIdFiles, {id:story.id, file:o}).then(() => refreshFiles())}
                            remove={o => call(api => api.storiesApi.deleteByIdFilesByFileId, { id: story.id, fileId: o }).then(() => refreshFiles())} />
                    )}
                    {/* Save button */}
                    <Box sx={{ display: 'flex', gap: 2, mt: 3 }}>
                        <Button type="submit" variant="contained" color="primary">
                            {t('opt.save')}
                        </Button>
                    </Box>
                </Grid>
            </Grid>
        </form>
    );
};

export default StoryEdit;
