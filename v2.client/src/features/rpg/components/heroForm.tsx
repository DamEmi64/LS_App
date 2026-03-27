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
import { HeroDto } from '@/features/rpg';
import { ImageProvider } from '@/shared/components/imageProvider';
import { useForm, Controller } from 'react-hook-form';
import noImage from '@/assets/no-image.png';
import { GridTable } from '@/shared/components/gridTable';
import { ColumnDef, ColumnType } from '@/shared';

type HeroViewEditProps = {
    hero: HeroDto;
    onSave?: (data: HeroDto) => void;
    onDelete?: (data: HeroDto) => void;
    isEdit?: boolean;
};

export const HeroForm: React.FC<HeroViewEditProps> = ({ hero, onSave, onDelete, isEdit }) => {
    const theme = useTheme();
    const textColor = theme.palette.mode === 'dark' ? theme.palette.text.primary : theme.palette.text.secondary;

    const [isEditing, setIsEditing] = useState(isEdit || false);
    const [image, setImage] = useState<string>(hero.image || noImage);

    const { control, handleSubmit, formState: { errors }, watch } = useForm<HeroDto>({
        defaultValues: { ...hero, image: hero.image || noImage },
    });
    
    const skillCol: ColumnDef[] = [
        { field: 'title', header: 'rpg.hero.skill.title', type: ColumnType.String },
        { field: 'value', header: 'rpg.hero.skill.value', type: ColumnType.String },
    ];

    const onSubmit = (data: HeroDto) => {
        data.image = image;
        if (onSave) onSave(data);
        setIsEditing(false);
    };

    // WATCH player field for conditional display
    const playerValue = watch('player');

    return (
        <Grid container spacing={2} alignItems="flex-start">
            {/* Left Side */}
            <Grid size={{ xs: 12, md: 8 }}>
                <Typography variant="h6" sx={{ color: textColor, mb: 2 }}>
                    {t('window.info')}
                </Typography>

                {isEditing ? (
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <Controller
                            name="firstName"
                            control={control}
                            rules={{ required: t('validation.required') as string }}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label={t('rpg.hero.firstName')}
                                    fullWidth
                                    margin="dense"
                                    variant="outlined"
                                    error={!!errors.firstName}
                                    helperText={errors.firstName?.message}
                                />
                            )}
                        />

                        <Controller
                            name="lastName"
                            control={control}
                            rules={{ required: t('validation.required') as string }}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label={t('rpg.hero.lastName')}
                                    fullWidth
                                    margin="dense"
                                    variant="outlined"
                                    error={!!errors.lastName}
                                    helperText={errors.lastName?.message}
                                />
                            )}
                        />

                        <Controller
                            name="player"
                            control={control}
                            render={({ field }) => (
                                <TextField {...field} label={t('rpg.hero.player')} fullWidth margin="dense" variant="outlined" />
                            )}
                        />

                        <Controller
                            name="description"
                            control={control}
                            rules={{ required: t('validation.required') as string }}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label={t('rpg.hero.description')}
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

                        {playerValue && (
                            <Controller
                                name="playerData"
                                control={control}
                                render={({ field }) => (
                                    <TextField
                                        {...field}
                                        label={t('rpg.hero.playerData')}
                                        multiline
                                        minRows={4}
                                        maxRows={16}
                                        fullWidth
                                        margin="dense"
                                        variant="outlined"
                                    />
                                )}
                            />
                        )}

                        <Box sx={{ display: 'flex', gap: 2, mt: 3 }}>
                            <Button type="submit" variant="contained" color="success">
                                {t('opt.save')}
                            </Button>
                            <Button type="button" variant="outlined" color="error" onClick={() => setIsEditing(false)}>
                                {t('opt.cancel')}
                            </Button>
                        </Box>
                    </form>
                ) : (
                    <>
                        <TextField
                            label={t('rpg.hero.firstName')}
                            value={hero.firstName}
                            fullWidth
                            InputProps={{ readOnly: true, style: { color: textColor } }}
                            margin="dense"
                            variant="outlined"
                        />
                        <TextField
                            label={t('rpg.hero.lastName')}
                            value={hero.lastName}
                            fullWidth
                            InputProps={{ readOnly: true, style: { color: textColor } }}
                            margin="dense"
                            variant="outlined"
                        />
                        <TextField
                            label={t('rpg.hero.player')}
                            value={hero.player || ''}
                            fullWidth
                            InputProps={{ readOnly: true, style: { color: textColor } }}
                            margin="dense"
                            variant="outlined"
                        />
                        <TextField
                            label={t('rpg.hero.description')}
                            value={hero.description}
                            multiline
                            minRows={4}
                            maxRows={16}
                            fullWidth
                            InputProps={{ readOnly: true, style: { color: textColor } }}
                            margin="dense"
                            variant="outlined"
                        />
                        {hero.player && (
                            <>
                            <TextField
                                label={t('rpg.hero.playerData')}
                                value={hero.playerData || ''}
                                multiline
                                minRows={4}
                                maxRows={16}
                                fullWidth
                                InputProps={{ readOnly: true, style: { color: textColor } }}
                                margin="dense"
                                variant="outlined"
                            />
                            <Typography variant="h6" sx={{ color: textColor, mt: 2 }}>
                                {t('rpg.hero.skills')}
                            </Typography>
                            <GridTable
                                columns={skillCol}
                                data={{ data: hero.skills, total: hero.skills.length }}
                            />
                            </>

                        )}

                        <Box sx={{ display: 'flex', gap: 2, mt: 3 }}>
                            <Button type="button" onClick={() => setIsEditing(true)} sx={{ background: theme.palette.primary.main, color: '#fff' }}>
                                {t('opt.edit')}
                            </Button>
                            {onDelete && (
                                <Button type="button" onClick={() => onDelete(hero)} sx={{ background: theme.palette.error.main, color: '#fff' }}>
                                    {t('opt.delete')}
                                </Button>
                            )}
                        </Box>
                    </>
                )}
            </Grid>

            {/* Right side: Image */}
            <Grid size={{ xs: 12, md: 4 }}>
                <ImageProvider imageId={hero.imageId} readonly={!isEditing} saveImage={setImage} />
            </Grid>
        </Grid>
    );
};

export default HeroForm;