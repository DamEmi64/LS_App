import React from 'react';
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

type SessionInfoProps = {
    story: Story;
    toSave?: (data: Story) => void;
};

export const StoryEdit: React.FC<SessionInfoProps> = ({ story, toSave }) => {
    const theme = useTheme();
    const textColor =
        theme.palette.mode === 'dark'
            ? theme.palette.text.primary
            : theme.palette.text.secondary;

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