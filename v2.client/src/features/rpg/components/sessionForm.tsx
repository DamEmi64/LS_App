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
import { SessionDto, Link } from '../types';
import { ImageProvider } from '@/shared/components/imageProvider';
import { useForm, Controller, useWatch } from 'react-hook-form';
import { GridTable } from '@/shared/components/gridTable';
import { ColumnDef, ColumnType, TableData } from '@/shared';
import noImage from '@/assets/no-image.png';

type SessionViewProps = {
    data: SessionDto;
    isEdit?: boolean;
    isNew?: boolean;
    isChapter?: boolean;
    onSave?: (data: SessionDto) => void;
    onDelete?: (data: SessionDto) => void;
    onCopy?: (data: SessionDto) => void;
};

export const SessionView: React.FC<SessionViewProps> = ({
    data,
    isEdit = false,
    isNew = false,
    isChapter,
    onSave,
    onDelete,
    onCopy
}) => {
    const theme = useTheme();
    const textColor = theme.palette.mode === 'dark' ? theme.palette.text.primary : theme.palette.text.secondary;

    const [editing, setEditing] = useState(isEdit || isNew);
    const [image, setImage] = useState<string>(data.image || noImage);

    const { control, handleSubmit, formState: { errors }, watch, setValue } = useForm<SessionDto>({
        defaultValues: { ...data, image: data.image || noImage },
    });

    const watchedLinks = useWatch({
    control,
    name: 'links'
    });

    const linkColumns: ColumnDef[] = [
        { field: 'title', header: 'rpg.other.title', type: ColumnType.String },
        { field: 'url', header: 'rpg.other.url', type: ColumnType.String },
    ];

    const sessionsColumns: ColumnDef[] = [
        { field: 'start', header: 'rpg.chapter.startDate', type: ColumnType.Date },
        { field: 'end', header: 'rpg.chapter.endDate', type: ColumnType.Date },
    ];

    const onSubmit = (data: SessionDto) => {
        data.image = image;
        if (onSave) onSave(data);
        setEditing(false);
    };

    return (
        <Grid container spacing={2} alignItems="flex-start">
            {/* Left side: Form */}
            <Grid size={data.imageId && !isChapter ? { xs: 12, md: 8 } : { xs: 12 }}>
                <Typography variant="h6" sx={{ color: textColor, mb: 2 }}>
                    {t('window.info')}
                </Typography>

                {editing ? (
                    <form onSubmit={handleSubmit(onSubmit)}>
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

                        {/* Chapter-only fields */}
                        {isChapter && (
                            <Controller
                                name="order"
                                control={control}
                                rules={{ required: t('validation.required') as string }}
                                render={({ field }) => (
                                    <TextField
                                        {...field}
                                        label={t('rpg.chapter.order')}
                                        type="number"
                                        fullWidth
                                        margin="dense"
                                        variant="outlined"
                                        error={!!errors.order}
                                        helperText={errors.order?.message}
                                    />
                                )}
                            />
                        )}

                        {/* Links */}
                        {isChapter && (
                            <>
                                <Typography variant="h6" sx={{ color: textColor, mt: 2 }}>
                                    {t('rpg.chapter.links')}
                                </Typography>
                                <GridTable
                                    columns={linkColumns}
                                    data={{ data: watchedLinks || [], total: watchedLinks?.length || 0 }}
                                    setData={(o) => setValue('links', o.data)}
                                />
                            </>
                        )}

                        {/* Save/Cancel buttons */}
                        <Box sx={{ display: 'flex', gap: 2, mt: 3 }}>
                            <Button type="submit" variant="contained" color="success">{t('opt.save')}</Button>
                            {!isNew && (
                                <Button type="button" variant="outlined" color="error" onClick={() => setEditing(false)}>{t('opt.cancel')}</Button>
                            )}
                        </Box>
                    </form>
                ) : (
                    <>
                        <TextField
                            label={t('rpg.other.title')}
                            value={data.title}
                            fullWidth
                            margin="dense"
                            InputProps={{ readOnly: true, style: { color: textColor } }}
                            variant="outlined"
                        />
                        <TextField
                            label={t('rpg.other.description')}
                            value={data.description}
                            multiline
                            minRows={4}
                            maxRows={16}
                            fullWidth
                            margin="dense"
                            InputProps={{ readOnly: true, style: { color: textColor } }}
                            variant="outlined"
                        />

                        {/* Sessions table */}
                        {data.sessions?.length && (
                            <>
                                <Typography variant="h6" sx={{ color: textColor, mt: 2 }}>
                                    {t('rpg.chapter.sessions')}
                                </Typography>
                                <GridTable
                                    columns={sessionsColumns}
                                    canDelete={false}
                                    data={{ data: data.sessions, total: data.sessions.length }}
                                />
                            </>
                        )}

                        {/* Links table */}
                        {data.links?.length && (
                            <>
                                <Typography variant="h6" sx={{ color: textColor, mt: 2 }}>
                                    {t('rpg.chapter.links')}
                                </Typography>
                                <GridTable
                                    columns={linkColumns}
                                    data={{ data: watchedLinks || [], total: watchedLinks?.length || 0 }}
                                    setData={(o) => setValue('links', o.data)}
                                />
                            </>
                        )}

                        <Box sx={{ display: 'flex', gap: 2, mt: 3 }}>
                            <Button type="button" onClick={() => setEditing(true)} sx={{ background: theme.palette.primary.main, color: '#fff' }}>
                                {t('opt.edit')}
                            </Button>

                            {onCopy && !isChapter && (
                                <Button
                                    type="button"
                                    onClick={() => onCopy(hero)}
                                    sx={{ background: theme.palette.secondary.main, color: '#fff' }}
                                >
                                    {t('opt.copy')}
                                </Button>
                            )}

                            {onDelete && (
                                <Button type="button" onClick={() => onDelete(data)} sx={{ background: theme.palette.error.main, color: '#fff' }}>
                                    {t('opt.delete')}
                                </Button>
                            )}
                        </Box>
                    </>
                )}
            </Grid>
            {!isChapter && (
                <Grid size={{ xs: 12, md: 4 }} sx={{ display: 'flex', justifyContent: 'flex-end', alignItems: 'flex-start' }}>
                    <ImageProvider
                        imageId={data.imageId}
                        readonly={!editing}
                    saveImage={editing ? setImage : undefined}
                />
            </Grid>
             )}
        </Grid>
    );
};

export default SessionView;