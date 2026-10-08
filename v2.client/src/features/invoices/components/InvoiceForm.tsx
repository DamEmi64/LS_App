import { useEffect, useState } from 'react';
import {
    Box,
    Button,
    CircularProgress,
    FormControl,
    Grid,
    IconButton,
    InputLabel,
    MenuItem,
    Select,
    Stack,
    TextField,
    Typography,
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import DeleteOutlineIcon from '@mui/icons-material/DeleteOutline';
import { useTranslation } from 'react-i18next';
import type { UserData } from '@/features/auth';
import type { Invoice, InvoiceSaveDto } from '../types';
import { loadInvoiceRecipients } from '../services/invoiceService';

type PositionDraft = { title: string; value: string; invoiceDate: string };
const getLocalDateInputValue = () => {
    const date = new Date();
    return `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}-${String(date.getDate()).padStart(2, '0')}`;
};
type InvoiceFormProps = {
    invoice?: Invoice;
    onSave: (data: InvoiceSaveDto) => Promise<void>;
    onCancel: () => void;
};

const InvoiceForm = ({ invoice, onSave, onCancel }: InvoiceFormProps) => {
    const { t } = useTranslation();
    const [users, setUsers] = useState<UserData[]>([]);
    const [title, setTitle] = useState(invoice?.title ?? '');
    const [recipientId, setRecipientId] = useState(invoice?.recipientId ?? '');
    const [positions, setPositions] = useState<PositionDraft[]>(() =>
        invoice?.positions.map(position => ({ title: position.title, value: String(position.value), invoiceDate: position.invoiceDate.slice(0, 10) })) ?? [{ title: '', value: '', invoiceDate: getLocalDateInputValue() }]
    );
    const [loadingUsers, setLoadingUsers] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState('');

    useEffect(() => {
        let active = true;
        loadInvoiceRecipients()
            .then(response => { if (active) setUsers(response.data ?? []); })
            .catch(() => { if (active) setError(t('invoices.usersLoadFailed')); })
            .finally(() => { if (active) setLoadingUsers(false); });
        return () => { active = false; };
    }, [t]);

    const setPosition = (index: number, field: keyof PositionDraft, value: string) => {
        setPositions(current => current.map((position, itemIndex) => itemIndex === index ? { ...position, [field]: value } : position));
    };

    const submit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        setError('');
        if (!title.trim() || !recipientId || positions.length === 0 || positions.some(position => !position.title.trim() || position.value === '' || !position.invoiceDate || Number(position.value) < 0)) {
            setError(t('invoices.completeRequiredFields'));
            return;
        }
        setSaving(true);
        try {
            await onSave({
                title: title.trim(),
                recipientId,
                positions: positions.map(position => ({ title: position.title.trim(), value: Number(position.value), invoiceDate: position.invoiceDate })),
            });
        } catch {
            setError(t('invoices.saveFailed'));
        } finally {
            setSaving(false);
        }
    };

    return (
        <Box component="form" onSubmit={submit} sx={{ width: { xs: 'min(92vw, 620px)', sm: 620 }, maxHeight: '80vh', overflowY: 'auto', p: 1 }}>
            <Typography variant="h6" sx={{ mb: 2 }}>{t(invoice ? 'invoices.editInvoice' : 'invoices.addInvoice')}</Typography>
            <TextField
                label={t('invoices.invoiceTitle')}
                value={title}
                onChange={event => setTitle(event.target.value)}
                fullWidth required margin="dense"
            />
            <FormControl fullWidth margin="dense" required>
                <InputLabel>{t('invoices.recipient')}</InputLabel>
                <Select
                    label={t('invoices.recipient')}
                    value={recipientId}
                    onChange={event => setRecipientId(event.target.value)}
                    disabled={loadingUsers}
                >
                    {users.map(user => (
                        <MenuItem key={user.userId} value={user.userId}>{user.login || user.email || user.userId}</MenuItem>
                    ))}
                </Select>
            </FormControl>

            <Stack direction="row" alignItems="center" justifyContent="space-between" sx={{ mt: 2, mb: 1 }}>
                <Typography variant="subtitle1" fontWeight="bold">{t('invoices.positions')}</Typography>
                <Button startIcon={<AddIcon />} onClick={() => setPositions(current => [...current, { title: '', value: '', invoiceDate: getLocalDateInputValue() }])}>
                    {t('invoices.addPosition')}
                </Button>
            </Stack>
            <Stack spacing={1}>
                {positions.map((position, index) => (
                    <Grid container spacing={1} key={index} alignItems="center">
                        <Grid size={{ xs: 12, sm: 5 }}>
                            <TextField
                                label={t('invoices.positionTitle')}
                                value={position.title}
                                onChange={event => setPosition(index, 'title', event.target.value)}
                                fullWidth required size="small"
                            />
                        </Grid>
                        <Grid size={{ xs: 5, sm: 3 }}>
                            <TextField
                                label={t('invoices.value')}
                                value={position.value}
                                onChange={event => setPosition(index, 'value', event.target.value)}
                                type="number" inputProps={{ min: 0, step: '0.01' }}
                                fullWidth required size="small"
                            />
                        </Grid>
                        <Grid size={{ xs: 5, sm: 3 }}>
                            <TextField
                                label={t('invoices.invoiceDate')}
                                value={position.invoiceDate}
                                onChange={event => setPosition(index, 'invoiceDate', event.target.value)}
                                type="date" fullWidth required size="small"
                                InputLabelProps={{ shrink: true }}
                            />
                        </Grid>
                        <Grid size={{ xs: 2, sm: 1 }}>
                            <IconButton
                                aria-label={t('invoices.removePosition')}
                                onClick={() => setPositions(current => current.filter((_, itemIndex) => itemIndex !== index))}
                            >
                                <DeleteOutlineIcon />
                            </IconButton>
                        </Grid>
                    </Grid>
                ))}
            </Stack>

            {error && <Typography color="error" variant="body2" sx={{ mt: 1 }}>{error}</Typography>}
            {loadingUsers && <Stack direction="row" spacing={1} alignItems="center" sx={{ mt: 1 }}><CircularProgress size={18} /><Typography>{t('invoices.loadingUsers')}</Typography></Stack>}
            <Stack direction="row" spacing={1} justifyContent="flex-end" sx={{ mt: 3 }}>
                <Button onClick={onCancel} disabled={saving}>{t('opt.cancel')}</Button>
                <Button type="submit" variant="contained" disabled={saving || loadingUsers}>
                    {saving ? <CircularProgress size={18} /> : t('opt.save')}
                </Button>
            </Stack>
        </Box>
    );
};

export default InvoiceForm;
