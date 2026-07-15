import { Box, Button, Grid, Stack, TextField, Typography, useMediaQuery, useTheme } from '@mui/material';
import { type FormEvent, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { ExpandableTable, Operations, ColumnType, TableColumn, useModal, call } from '@/shared';
import { DiscordCmd } from '@/shared/api/generated';
import { ResponseList } from '@/shared/api/extension';

type DiscordCommandRow = {
    id: string;
    command: string;
    response: string;
    active: boolean;
};

type EditResponseModalProps = {
    row: DiscordCommandRow;
    onSave: (value: string) => void;
    onClose: () => void;
};

const EditResponseModal = ({ row, onSave, onClose }: EditResponseModalProps) => {
    const { t } = useTranslation();
    const [value, setValue] = useState(row.response);

    const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        onSave(value);
    };

    return (
        <Box component="form" onSubmit={handleSubmit} sx={{ minWidth: { xs: 280, sm: 420 } }}>
            <Typography variant="h6" sx={{ mb: 2 }}>
                {t('discord.editResponseModal.title', { command: row.command })}
            </Typography>
            <TextField
                fullWidth
                multiline
                minRows={5}
                value={value}
                onChange={(event) => setValue(event.target.value)}
                label={t('discord.response')}
                sx={{ mb: 2 }}
            />
            <Stack direction="row" spacing={1} justifyContent="flex-end">
                <Button onClick={onClose}>{t('opt.cancel')}</Button>
                <Button type="submit" variant="contained">{t('opt.save')}</Button>
            </Stack>
        </Box>
    );
};

const DiscordPage = () => {
    const { t } = useTranslation();
    const modal = useModal();
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('sm'));
    const [rows, setRows] = useState<DiscordCommandRow[]>([]);

    const loadCommands = async () => {
        const data = await call<ResponseList<DiscordCmd>>(api => api.discordClient.get, {});
        setRows((data.data || []).map((item) => ({
            id: item.id || '',
            command: item.cmd || '',
            response: item.response || '',
            active: item.active || false,
        })));
    };

    const saveCommand = async (row: DiscordCommandRow, updates: Partial<DiscordCommandRow>) => {
        const payload: DiscordCmd = {
            id: row.id,
            cmd: row.command,
            response: updates.response ?? row.response,
            active: updates.active ?? row.active,
        };

        await call(api => api.discordClient.updateById, { id: row.id, discordCmd: payload });
        await loadCommands();
    };

    const handleToggle = async (row: DiscordCommandRow) => {
        await saveCommand(row, { active: !row.active });
    };

    const openEditModal = (row: DiscordCommandRow) => {
        modal.showModal(
            <EditResponseModal
                row={row}
                onClose={modal.hideModal}
                onSave={async (value) => {
                    await saveCommand(row, { response: value });
                    modal.hideModal();
                }}
            />
        );
    };

    const operations: Operations<DiscordCommandRow>[] = [
        {
            name: 'discord.enable',
            method: handleToggle,
            hidden: (row) => row.active,
        },
        {
            name: 'discord.disable',
            method: handleToggle,
            hidden: (row) => !row.active,
        },
        {
            name: 'discord.editResponse',
            method: openEditModal,
        },
    ];

    const columns: TableColumn<DiscordCommandRow>[] = [
        { field: 'command', header: 'discord.command', type: ColumnType.String, sortable: false },
        { field: 'response', header: 'discord.response', type: ColumnType.String, sortable: false },
        { field: 'active', header: 'discord.active', type: ColumnType.Boolean, sortable: false },
    ];

    useEffect(() => {
        void loadCommands();
    }, []);

    return (
        <Grid container sx={{ width: '100%', p: { xs: 1, md: 3 } }}>
            <Grid size={{ xs: 12 }} sx={{ mb: 2, display: 'flex', flexDirection: 'column', alignItems: 'center', textAlign: 'center' }}>
                <Typography
                    sx={{
                        mb: 1,
                        color: theme.palette.primary.main,
                        fontSize: isMobile ? '1.8rem' : '2.5rem',
                        fontWeight: 'bold',
                    }}
                >
                    {t('discord.title')}
                </Typography>
                <Typography sx={{ color: theme.palette.text.secondary }}>
                    {t('discord.description')}
                </Typography>
            </Grid>
            <Grid size={{ xs: 12 }}>
                <ExpandableTable
                    rows={rows}
                    columns={columns}
                    operations={operations}
                    getRowId={(row) => row.id}
                />
            </Grid>
        </Grid>
    );
};

export default DiscordPage;
