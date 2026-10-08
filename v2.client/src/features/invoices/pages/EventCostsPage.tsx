import { useCallback, useEffect, useState } from 'react';
import { Accordion, AccordionDetails, AccordionSummary, Box, Button, Chip, CircularProgress, Paper, Stack, Table, TableBody, TableCell, TableHead, TableRow, Typography, useMediaQuery, useTheme } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import EventNoteIcon from '@mui/icons-material/EventNote';
import ReceiptLongIcon from '@mui/icons-material/ReceiptLong';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { useTranslation } from 'react-i18next';
import { useModal } from '@/shared/context/modal';
import { notify } from '@/shared/components/NotificationListener';
import { createEventInvoices, loadEventInvoices, type EventInvoiceRow } from '../services/eventInvoiceService';
import EventInvoiceForm from '../components/EventInvoiceForm';

const EventCostsPage = () => {
    const { t } = useTranslation();
    const modal = useModal();
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('sm'));
    const [rows, setRows] = useState<EventInvoiceRow[]>([]);
    const [loading, setLoading] = useState(true);
    const load = useCallback(async () => {
        setLoading(true);
        try { setRows(await loadEventInvoices()); }
        catch { notify('error', t('eventCosts.loadFailed')); }
        finally { setLoading(false); }
    }, [t]);
    useEffect(() => { void load(); }, [load]);

    const eventGroups = Object.values(rows.reduce<Record<string, { eventId?: string; eventTitle?: string; eventDate?: string; rows: EventInvoiceRow[] }>>((groups, row) => {
        const key = row.eventId ?? row.eventTitle ?? 'unknown-event';
        groups[key] ??= { eventId: row.eventId, eventTitle: row.eventTitle, eventDate: row.eventDate, rows: [] };
        groups[key].rows.push(row);
        return groups;
    }, {}));

    const add = () => modal.showModal(<EventInvoiceForm onCancel={modal.hideModal} onSave={async draft => {
        await createEventInvoices(draft);
        modal.hideModal();
        await load();
    }} />);

    return <Box sx={{ width: '100%', p: { xs: 1, sm: 3 } }}>
        <Stack alignItems="center" spacing={1} sx={{ mb: 3, textAlign: 'center' }}>
            <Typography variant={isMobile ? 'h4' : 'h3'} fontWeight="bold">{t('eventCosts.title')}</Typography>
            <Typography color="text.secondary">{t('eventCosts.description')}</Typography>
            <Button startIcon={<AddIcon />} variant="outlined" onClick={add} fullWidth={isMobile} sx={{ mt: 1 }}>
                {t('eventCosts.add')}
            </Button>
        </Stack>

        {loading ? <Box sx={{ display: 'flex', justifyContent: 'center', py: 5 }}><CircularProgress /></Box> : eventGroups.length === 0 ?
            <Paper sx={{ p: 4, textAlign: 'center' }}><EventNoteIcon color="disabled" sx={{ fontSize: 42, mb: 1 }} /><Typography color="text.secondary">{t('eventCosts.empty')}</Typography></Paper> :
            <Stack spacing={1.5}>
                {eventGroups.map(group => {
                    const total = group.rows.reduce((sum, row) => sum + Number(row.value ?? 0), 0);
                    return <Accordion key={group.eventId ?? group.eventTitle} disableGutters sx={{ borderRadius: 1, '&:before': { display: 'none' }, overflow: 'hidden' }}>
                        <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                            <Stack direction="row" spacing={1.5} alignItems="center" sx={{ width: '100%', pr: 1 }}>
                                <EventNoteIcon color="primary" />
                                <Box sx={{ flexGrow: 1, minWidth: 0 }}>
                                    <Typography fontWeight="bold" noWrap>{group.eventTitle ?? group.eventId}</Typography>
                                    {group.eventDate && <Typography variant="body2" color="text.secondary">{new Date(group.eventDate).toLocaleDateString()}</Typography>}
                                </Box>
                                <Chip icon={<ReceiptLongIcon />} label={`${group.rows.length} · ${total.toFixed(2)}`} color="primary" variant="outlined" />
                            </Stack>
                        </AccordionSummary>
                        <AccordionDetails sx={{ pt: 0 }}>
                            <Box sx={{ overflowX: 'auto' }}><Table size="small"><TableHead><TableRow>
                                <TableCell>{t('eventCosts.user')}</TableCell><TableCell align="right">{t('eventCosts.value')}</TableCell>
                            </TableRow></TableHead><TableBody>{group.rows.map((row, index) => <TableRow key={`${row.invoiceId}-${index}`}>
                                <TableCell>{row.userLogin ?? row.userId}</TableCell><TableCell align="right">{Number(row.value ?? 0).toFixed(2)}</TableCell>
                            </TableRow>)}</TableBody></Table></Box>
                        </AccordionDetails>
                    </Accordion>;
                })}
            </Stack>}
    </Box>;
};

export default EventCostsPage;
