import { useEffect, useState } from 'react';
import { Box, Button, CircularProgress, FormControl, Grid, InputLabel, MenuItem, Select, Stack, TextField, Typography, useTheme } from '@mui/material';
import { useTranslation } from 'react-i18next';
import type { EventDto } from '@/shared/api/generated';
import { ColumnType, GridTable, type TableColumn, type TableData } from '@/shared';
import { loadEventsForCosts, type EventInvoiceDraft } from '../services/eventInvoiceService';

type Props = { onSave: (draft: EventInvoiceDraft) => Promise<void>; onCancel: () => void };
type Allocation = { userId: string; label: string; value: string };
type PositionRow = { id: number; positionTitle: string; value: number | string };

const EventInvoiceForm = ({ onSave, onCancel }: Props) => {
    const { t } = useTranslation();
    const theme = useTheme();
    const textColor = theme.palette.text.primary;
    const [events, setEvents] = useState<EventDto[]>([]);
    const [eventId, setEventId] = useState('');
    const [title, setTitle] = useState('');
    const [positions, setPositions] = useState<PositionRow[]>([]);
    const [allocations, setAllocations] = useState<Allocation[]>([]);
    const [equalSplit, setEqualSplit] = useState(true);
    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState('');

    useEffect(() => {
        let active = true;
        loadEventsForCosts().then(data => { if (active) setEvents(data); })
            .catch(() => { if (active) setError(t('eventCosts.eventsLoadFailed')); })
            .finally(() => { if (active) setLoading(false); });
        return () => { active = false; };
    }, [t]);

    const totalValue = positions.reduce((sum, position) => sum + (Number(position.value) || 0), 0);

    useEffect(() => {
        if (!equalSplit || allocations.length === 0) return;
        const share = (totalValue / allocations.length).toFixed(2);
        setAllocations(current => current.map(allocation => ({ ...allocation, value: share })));
    }, [totalValue, equalSplit]);

    const chooseEvent = (id: string) => {
        setEventId(id);
        setPositions([]);
        setEqualSplit(true);
        const selected = events.find(event => event.id === id);
        const participants = (selected?.participates ?? []).filter(user => user.userId);
        setAllocations(participants.map(user => ({ userId: user.userId!, label: user.login || user.email || user.userId!, value: '0.00' })));
    };

    const positionColumns: TableColumn<PositionRow>[] = [
        { field: 'positionTitle', header: 'eventCosts.position', type: ColumnType.String },
        { field: 'value', header: 'eventCosts.value', type: ColumnType.Number },
    ];

    const updatePositions = (data: TableData<PositionRow>) => setPositions(data.data);

    const submit = async (e: React.FormEvent) => {
        e.preventDefault(); setError('');
        if (!eventId || !title.trim() || positions.length === 0 || positions.some(position =>
            !position.positionTitle?.trim() || position.value === '' || !Number.isFinite(Number(position.value)) || Number(position.value) < 0
        ) || allocations.length === 0 || allocations.some(allocation =>
            allocation.value === '' || !Number.isFinite(Number(allocation.value)) || Number(allocation.value) < 0
        )) {
            setError(t('eventCosts.required')); return;
        }

        const allocationTotal = allocations.reduce((sum, allocation) => sum + Number(allocation.value), 0);
        setSaving(true);
        try {
            await onSave({
                eventId,
                title: title.trim(),
                positions: positions.map(position => ({
                    positionTitle: position.positionTitle.trim(),
                    allocations: allocations.map(allocation => ({
                        userId: allocation.userId,
                        value: Math.round((allocationTotal > 0
                            ? Number(position.value) * Number(allocation.value) / allocationTotal
                            : 0) * 100) / 100,
                    })),
                })),
            });
        } catch { setError(t('eventCosts.saveFailed')); }
        finally { setSaving(false); }
    };

    return <Box component="form" onSubmit={submit} sx={{ width: { xs: '94vw', sm: 600 }, maxWidth: 600, maxHeight: '82vh', overflowY: 'auto', p: 1 }}>
        <Typography variant="h6" sx={{ mb: 2 }}>{t('eventCosts.add')}</Typography>
        <FormControl fullWidth required margin="dense"><InputLabel>{t('eventCosts.event')}</InputLabel><Select value={eventId} label={t('eventCosts.event')} onChange={e => chooseEvent(e.target.value)} disabled={loading}>
            {events.map(event => <MenuItem key={event.id} value={event.id}>{event.title} {event.eventDate ? `(${new Date(event.eventDate).toLocaleDateString()})` : ''}</MenuItem>)}
        </Select></FormControl>
        <TextField fullWidth required margin="dense" label={t('eventCosts.invoiceTitle')} value={title} onChange={e => setTitle(e.target.value)} />

        <Typography variant="subtitle1" fontWeight="bold" color= {textColor} sx={{ mt: 2, mb: 1 }}>{t('eventCosts.positions')}</Typography>
        <Box sx={{ width: '100%', maxWidth: 540 }}>
            <GridTable<PositionRow>
                columns={positionColumns}
                data={{ data: positions, total: positions.length }}
                setData={updatePositions}
            />
        </Box>
        <Typography variant="body2" color= {textColor} align="right" sx={{ mt: 1 }}>
            {t('eventCosts.totalValue')}: {totalValue.toFixed(2)}
        </Typography>

        <Typography color= {textColor} variant="subtitle1" fontWeight="bold" sx={{ mt: 2, mb: 1 }}>{t('eventCosts.shares')}</Typography>
        {allocations.length === 0 ? <Typography color= {textColor}>{t('eventCosts.noParticipants')}</Typography> : <Stack spacing={1}>{allocations.map((allocation, index) => <Grid container spacing={1} alignItems="center" key={allocation.userId}>
            <Grid size={{ xs: 7 }}><Typography color= {textColor}>{allocation.label}</Typography></Grid>
            <Grid size={{ xs: 5 }}><TextField label={t('eventCosts.value')} value={allocation.value} type="number" inputProps={{ min: 0, step: '0.01', color: textColor }} fullWidth size="small" onChange={e => {
                setEqualSplit(false);
                setAllocations(current => current.map((item, i) => i === index ? { ...item, value: e.target.value } : item));
            }} /></Grid>
        </Grid>)}</Stack>}
        {allocations.length > 0 && !equalSplit && <Button size="small" onClick={() => setEqualSplit(true)} sx={{ mt: 1 }}>{t('eventCosts.splitEqually')}</Button>}
        {loading && <Stack direction="row" spacing={1} alignItems="center" sx={{ mt: 1 }}><CircularProgress size={18} /><Typography>{t('eventCosts.loading')}</Typography></Stack>}
        {error && <Typography color="error" variant="body2" sx={{ mt: 1 }}>{error}</Typography>}
        <Stack direction="row" spacing={1} justifyContent="flex-end" sx={{ mt: 3 }}><Button onClick={onCancel} disabled={saving}>{t('opt.cancel')}</Button><Button type="submit" variant="contained" disabled={saving || loading || allocations.length === 0}>{saving ? <CircularProgress size={18} /> : t('opt.save')}</Button></Stack>
    </Box>;
};

export default EventInvoiceForm;
