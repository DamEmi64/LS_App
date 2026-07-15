import { Box, Button, Grid, Stack, TextField, Typography } from '@mui/material';
import { type FormEvent, useMemo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { ExpandableTable, Operations, ColumnType, TableColumn, useModal } from '@/shared';

type DiscordCommandRow = {
    id: string;
    command: string;
    response: string;
    active: boolean;
};

const initialRows: DiscordCommandRow[] = [
    { id: '1', command: '!help', response: 'List of available commands', active: true },
    { id: '2', command: '!status', response: 'Server is running normally', active: false },
    { id: '3', command: '!ping', response: 'Pong!', active: true },
];

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
    const [rows, setRows] = useState<DiscordCommandRow[]>(initialRows);

    const handleToggle = (row: DiscordCommandRow) => {
        setRows((currentRows) =>
            currentRows.map((item) =>
                item.id === row.id ? { ...item, active: !item.active } : item
            )
        );
        // TODO: send the updated active state to the server.
    };

    const openEditModal = (row: DiscordCommandRow) => {
        modal.showModal(
            <EditResponseModal
                row={row}
                onClose={modal.hideModal}
                onSave={(value) => {
                    setRows((currentRows) =>
                        currentRows.map((item) =>
                            item.id === row.id ? { ...item, response: value } : item
                        )
                    );
                    // TODO: send the updated response to the server.
                    modal.hideModal();
                }}
            />
        );
    };

    const operations = useMemo<Operations<DiscordCommandRow>[]>(() => [
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
    ], [modal]);

    const columns: TableColumn<DiscordCommandRow>[] = [
        { field: 'command', header: 'discord.command', type: ColumnType.String, sortable: true },
        { field: 'response', header: 'discord.response', type: ColumnType.String, sortable: true },
        { field: 'active', header: 'discord.active', type: ColumnType.Boolean, sortable: true },
    ];

    return (
        <Grid container sx={{ width: '100%', p: { xs: 1, md: 3 } }}>
            <Grid size={{ xs: 12 }} sx={{ mb: 2 }}>
                <Typography variant="h4" sx={{ mb: 1 }}>
                    {t('discord.title')}
                </Typography>
                <Typography color="text.secondary">
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
