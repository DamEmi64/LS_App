import React, { useState, useEffect } from 'react';
import { Box, TextField, Typography, useTheme, Grid, Button } from '@mui/material';
import { t } from 'i18next';
import { HeroDto, Skill } from '@/features/rpg';
import { ColumnType, TableColumn, TableData } from '@/shared';
import { GridTable } from '@/shared/components/datatables/gridTable';

type PlayerWindowProps = {
    hero: HeroDto;
    toSave?: (data: HeroDto) => void;
};

export const PlayerWindow: React.FC<PlayerWindowProps> = ({ hero, toSave }) => {
    const theme = useTheme();
    const textColor = theme.palette.mode === 'dark' ? theme.palette.text.primary : theme.palette.text.secondary;

    const skillCol: TableColumn<Skill>[] = [
        { field: 'title', header: 'rpg.hero.skill.title', type: ColumnType.String },
        { field: 'value', header: 'rpg.hero.skill.value', type: ColumnType.String },
    ];

    const [formData, setFormData] = useState<HeroDto>(hero);

    useEffect(() => {
        setFormData(hero);
    }, [hero]);

    const handleSubmit = () => {
        if (toSave) toSave(formData);
    };

    return (
        <Grid container spacing={2}>
            <Grid size={{ xs: 12 }}>
                <TextField
                    label={t('rpg.hero.playerData')}
                    value={formData.playerData || ''}
                    multiline
                    minRows={4}
                    maxRows={16}
                    onChange={(event) => setFormData({ ...formData, playerData: event.target.value })}
                    InputProps={{ style: { color: textColor } }}
                    fullWidth
                    margin="dense"
                    variant="outlined"
                />
            </Grid>

            <Grid size={{ xs: 12 }}>
                <GridTable
                    columns={skillCol}
                    data={{ data: formData.skills || [], total: formData.skills?.length || 0 } as TableData<Skill>}
                    setData={(o) => setFormData({ ...formData, skills: o.data })}
                />
            </Grid>

            <Grid size={{ xs: 12 }}>
                <Box sx={{ display: 'flex', gap: 2, mt: 3 }}>
                    <Button
                        type="button"
                        onClick={handleSubmit}
                        sx={{ padding: '8px 16px', background: theme.palette.primary.main, color: '#fff', borderRadius: 2 }}
                    >
                        {t('opt.save')}
                    </Button>
                </Box>
            </Grid>
        </Grid>
    );
};

export default PlayerWindow;