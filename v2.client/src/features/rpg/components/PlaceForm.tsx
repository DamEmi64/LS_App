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
import { ImageProvider } from '@/shared/components/imageProvider';
import { ColumnType, TableColumn, TableData } from '@/shared';
import { useForm, Controller } from 'react-hook-form';
import { SessionDto, Link } from '../types';
import noImage from '@/assets/no-image.png';

type PlaceForm = {
    data: SessionDto; // you may want to rename this to PlaceDto later
    onSave?: (o: SessionDto) => void;
    onDelete?: (o: SessionDto) => void;
    isEdit?: boolean;
    isNew?: boolean;
};

export const PlaceForm: React.FC<PlaceForm> = ({
    data,
    onSave,
    onDelete,
    isEdit,
    isNew
}) => {
    const theme = useTheme();
    const textColor =
        theme.palette.mode === 'dark'
            ? theme.palette.text.primary
            : theme.palette.text.secondary;

    const [editing, setEditing] = useState(isEdit || isNew || false);
    const [image, setImage] = useState<string>(data.image || noImage);

    const { control, handleSubmit, formState: { errors }, watch, setValue } =
        useForm<SessionDto>({
            defaultValues: { ...data, image: data.image || noImage },
        });

    const linkColumns: TableColumn<Link>[] = [
        { field: 'title', header: 'rpg.other.title', type: ColumnType.String },
        { field: 'url', header: 'rpg.other.url', type: ColumnType.String },
    ];

    const onSubmit = (formData: SessionDto) => {
        formData.image = image;
        onSave?.(formData);
        setEditing(false);
    };

    return (
        <Grid container spacing={2} alignItems="flex-start">
            {/* LEFT */}
            <Grid size={data.imageId ? { xs: 12, md: 8 } : { xs: 12 }}>
                <Typography variant="h6" sx={{ color: textColor, mb: 2 }}>
                    {t('window.info')}
                </Typography>

                {editing ? (
                    <form onSubmit={handleSubmit(onSubmit)}>
                        {/* TITLE */}
                        <Controller
                            name="title"
                            control={control}
                            rules={{ required: t('validation.required') as string }}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label={t('rpg.other.title')}
                                    fullWidth
                                    margin="dense"
                                    variant="outlined"
                                    error={!!errors.title}
                                    helperText={errors.title?.message}
                                />
                            )}
                        />

                        {/* DESCRIPTION */}
                        <Controller
                            name="description"
                            control={control}
                            rules={{ required: t('validation.required') as string }}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label={t('rpg.other.description')}
                                    multiline
                                    minRows={4}
                                    maxRows={16}
                                    fullWidth
                                    margin="dense"
                                    variant="outlined"
                                    error={!!errors.description}
                                    helperText={errors.description?.message}
                                />
                            )}
                        />

                        {/* ACTIONS */}
                        <Box sx={{ display: 'flex', gap: 2, mt: 3 }}>
                            <Button type="submit" variant="contained" color="success">
                                {t('opt.save')}
                            </Button>
                            {!isNew && (
                                <Button
                                    type="button"
                                    variant="outlined"
                                    color="error"
                                    onClick={() => setEditing(false)}
                                >
                                    {t('opt.cancel')}
                                </Button>
                            )}
                        </Box>
                    </form>
                ) : (
                    <>
                        {/* VIEW MODE */}
                        <TextField
                            label={t('rpg.other.title')}
                            value={data.title}
                            fullWidth
                            margin="dense"
                            variant="outlined"
                            InputProps={{ readOnly: true }}
                        />

                        <TextField
                            label={t('rpg.other.description')}
                            value={data.description}
                            multiline
                            minRows={4}
                            maxRows={16}
                            fullWidth
                            margin="dense"
                            variant="outlined"
                            InputProps={{ readOnly: true }}
                        />

                        {/* ACTIONS */}
                        <Box sx={{ display: 'flex', gap: 2, mt: 3 }}>
                            <Button onClick={() => setEditing(true)} variant="contained">
                                {t('opt.edit')}
                            </Button>

                            {onDelete && (
                                <Button
                                    onClick={() => onDelete(data)}
                                    variant="contained"
                                    color="error"
                                >
                                    {t('opt.delete')}
                                </Button>
                            )}
                        </Box>
                    </>
                )}
            </Grid>

            {/* RIGHT (IMAGE) */}
            <Grid
                size={{ xs: 12, md: 4 }}
                sx={{ display: 'flex', justifyContent: 'flex-end' }}
            >
                <ImageProvider
                    imageId={data.imageId || ''}
                    readonly={!editing}
                    saveImage={editing ? setImage : undefined}
                />
            </Grid>
        </Grid>
    );
};

export default PlaceForm;