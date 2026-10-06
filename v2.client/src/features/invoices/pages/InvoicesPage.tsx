import { useCallback, useEffect, useMemo, useState } from 'react';
import {
    Box,
    Button,
    CircularProgress,
    Grid,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
    Typography,
    Dialog, DialogTitle, DialogContent, DialogActions, FormControl, InputLabel, MenuItem, Select, TextField,
} from '@mui/material';
import { useTranslation } from 'react-i18next';
import YesNoWindow from '@/shared/components/YesNoWindow';
import {
    ColumnType,
    ExpandableTable,
    FilterItem,
    FilterType,
    FilterValue,
    Operations,
    TableColumn,
    useModal,
} from '@/shared';
import { notify } from '@/shared/components/NotificationListener';
import { createInvoice, deleteInvoice, generateInvoiceDocument, getInvoicesToPay, getMyInvoices, markInvoicePositionPaid, updateInvoice } from '../services/invoiceService';
import { INVOICE_STATUS, Invoice, InvoicePosition, InvoiceSaveDto } from '../types';
import InvoiceForm from '../components/InvoiceForm';
import CreditScoreIcon from '@mui/icons-material/CreditScore';
import CreditCardOffIcon from '@mui/icons-material/CreditCardOff';

type InvoicesPageProps = { mode: 'mine' | 'toPay' };
type InvoiceRow = Invoice & { visiblePositions: InvoicePosition[] };
type PositionOperationRow = InvoicePosition & { invoiceId: string };

const InvoicesPage = ({ mode }: InvoicesPageProps) => {
    const { t } = useTranslation();
    const modal = useModal();
    const [invoices, setInvoices] = useState<Invoice[]>([]);
    const [filters, setFilters] = useState<FilterValue[]>([]);
    const [loading, setLoading] = useState(true);
    const [documentInvoice, setDocumentInvoice] = useState<InvoiceRow | null>(null);
    const [paymentMethod, setPaymentMethod] = useState(1);
    const [accountNumber, setAccountNumber] = useState('');
    const [generating, setGenerating] = useState(false);

    const loadInvoices = useCallback(async () => {
        setLoading(true);
        try {
            setInvoices(mode === 'mine' ? await getMyInvoices() : await getInvoicesToPay());
        } catch {
            setInvoices([]);
        } finally {
            setLoading(false);
        }
    }, [mode]);

    useEffect(() => { void loadInvoices(); }, [loadInvoices]);

    const filterValues = Object.fromEntries(filters.map(({ field, value }) => [field, value])) as Record<string, string>;
    const rows = useMemo<InvoiceRow[]>(() => invoices.flatMap((invoice) => {
        const title = (filterValues.title || '').trim().toLocaleLowerCase();
        const recipient = (filterValues.recipient || '').trim().toLocaleLowerCase();
        const paidFilter = filterValues.paid || '';
        if (recipient && !invoice.recipientLogin.toLocaleLowerCase().includes(recipient)) return [];

        let positions = invoice.positions;
        if (paidFilter === 'paid') positions = positions.filter(position => position.status === INVOICE_STATUS.paid);
        if (paidFilter === 'unpaid') positions = positions.filter(position => position.status !== INVOICE_STATUS.paid);
        if (title) {
            const invoiceTitleMatches = invoice.title.toLocaleLowerCase().includes(title);
            if (!invoiceTitleMatches) positions = positions.filter(position => position.title.toLocaleLowerCase().includes(title));
            if (!invoiceTitleMatches && positions.length === 0) return [];
        }
        if (paidFilter && positions.length === 0) return [];
        return [{ ...invoice, visiblePositions: positions }];
    }), [invoices, filterValues.title, filterValues.recipient, filterValues.paid]);

    const filterItems: FilterItem[] = [
        { field: 'title', name: 'invoices.filters.title', type: FilterType.String },
        { field: 'recipient', name: 'invoices.filters.recipient', type: FilterType.String },
        {
            field: 'paid', name: 'invoices.filters.paymentStatus', type: FilterType.Enum,
            options: [
                { label: 'invoices.unpaid', value: 'unpaid' },
                { label: 'invoices.paid', value: 'paid' },
            ],
        },
    ];

    const columns: TableColumn<InvoiceRow>[] = [
        { field: 'title', header: 'invoices.invoice', type: ColumnType.String },
        {
            field: mode === 'mine' ? 'recipientLogin' : 'collectorLogin',
            header: mode === 'mine' ? 'invoices.recipient' : 'invoices.collector',
            type: ColumnType.String,
        },
        {
            field: 'total', header: 'invoices.total', type: ColumnType.Number,
            render: row => row.visiblePositions.reduce((sum, position) => sum + position.value, 0).toFixed(2),
        },
    ];

    const markPaid = async (invoiceId: string, position: InvoicePosition) => {
        try {
            if (position.status == INVOICE_STATUS.paid) return;
            await markInvoicePositionPaid(invoiceId, position.id);
            await loadInvoices();
        } catch {
            notify('error', t('invoices.paymentFailed'));
        }
    };

    const openAddInvoice = () => {
        modal.showModal(
            <InvoiceForm
                onCancel={modal.hideModal}
                onSave={async (data: InvoiceSaveDto) => {
                    await createInvoice(data);
                    modal.hideModal();
                    await loadInvoices();
                }}
            />
        );
    };

    const openEditInvoice = (invoice: InvoiceRow) => {
        modal.showModal(
            <InvoiceForm
                invoice={invoice}
                onCancel={modal.hideModal}
                onSave={async (data: InvoiceSaveDto) => {
                    await updateInvoice(invoice.id, data);
                    modal.hideModal();
                    await loadInvoices();
                }}
            />
        );
    };

    const confirmDeleteInvoice = (invoice: InvoiceRow) => {
        modal.showModal(
            <YesNoWindow
                message="invoices.deleteConfirm"
                open
                onClose={modal.hideModal}
                noMethod={modal.hideModal}
                yesMethod={async () => {
                    await deleteInvoice(invoice.id);
                    await loadInvoices();
                }}
            />
        );
    };

    const generateDocument = async () => {
        if (!documentInvoice) return;
        setGenerating(true);
        try {
            await generateInvoiceDocument(documentInvoice.id, paymentMethod, paymentMethod === 2 ? accountNumber : undefined);
            setDocumentInvoice(null);
        } catch {
            notify('error', t('invoices.generateFailed'));
        } finally {
            setGenerating(false);
        }
    };

    const invoiceOperations: Operations<InvoiceRow>[] = mode === 'mine' ? [
        { name: 'invoices.generateInvoice', method: invoice => { setPaymentMethod(1); setAccountNumber(''); setDocumentInvoice(invoice); } },
        { name: 'opt.edit', method: openEditInvoice },
        { name: 'opt.delete', method: confirmDeleteInvoice },
    ] : [];

    return (
        <Grid container sx={{ width: '100%', p: { xs: 1, md: 3 } }}>
            <Grid size={{ xs: 12 }} sx={{ mb: 2, textAlign: 'center' }}>
                <Typography variant="h4" color="primary" fontWeight="bold">
                    {t(mode === 'mine' ? 'invoices.myInvoices' : 'invoices.toPay')}
                </Typography>
                <Typography color="text.secondary" sx={{ mt: 0.5 }}>
                    {t(mode === 'mine' ? 'invoices.myInvoicesDescription' : 'invoices.toPayDescription')}
                </Typography>
                {mode === 'mine' && (
                    <Button variant="contained" sx={{ mt: 2 }} onClick={openAddInvoice}>
                        {t('invoices.addInvoice')}
                    </Button>
                )}
            </Grid>
            <Grid size={{ xs: 12 }}>
                {loading ? (
                    <Box display="flex" justifyContent="center" py={6}><CircularProgress /></Box>
                ) : invoices.length === 0 ? (
                    <Paper sx={{ p: 4, textAlign: 'center' }}><Typography color="text.secondary">{t('invoices.empty')}</Typography></Paper>
                ) : (
                    <ExpandableTable
                        rows={rows}
                        columns={columns}
                        operations={invoiceOperations}
                        getRowId={row => row.id}
                        filters={filterItems}
                        onFilterChange={setFilters}
                        renderExpanded={invoice => (
                            <Table size="small" aria-label={t('invoices.positions')}>
                                <TableHead>
                                    <TableRow>
                                        <TableCell>{t('invoices.position')}</TableCell>
                                        <TableCell align="right">{t('invoices.value')}</TableCell>
                                        <TableCell>{t('invoices.paymentStatus')}</TableCell>
                                        {mode === 'toPay' && <TableCell align="right">{t('invoices.action')}</TableCell>}
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {invoice.visiblePositions.map(position => {
                                        const paid = position.status === INVOICE_STATUS.paid;
                                        return (
                                            <TableRow key={position.id}>
                                                <TableCell>{position.title}</TableCell>
                                                <TableCell align="right">{position.value.toFixed(2)}</TableCell>
                                                <TableCell onClick={o => markPaid(invoice.id, position)}>{paid ? <CreditScoreIcon/> : <CreditCardOffIcon/>}</TableCell>
                                            </TableRow> 
                                        );
                                    })}
                                </TableBody>
                            </Table>
                        )}
                    />
                )}
            </Grid>
            <Dialog open={documentInvoice !== null} onClose={() => !generating && setDocumentInvoice(null)} fullWidth maxWidth="xs">
                <DialogTitle>{t('invoices.generateInvoice')}</DialogTitle>
                <DialogContent sx={{ display: 'grid', gap: 2, pt: '12px !important' }}>
                    <FormControl fullWidth>
                        <InputLabel id="invoice-payment-label">{t('invoices.paymentMethod')}</InputLabel>
                        <Select labelId="invoice-payment-label" label={t('invoices.paymentMethod')} value={paymentMethod} onChange={event => setPaymentMethod(Number(event.target.value))}>
                            <MenuItem value={0}>--</MenuItem>
                            <MenuItem value={1}>{t('invoices.paymentBlik')}</MenuItem>
                            <MenuItem value={2}>{t('invoices.paymentAccount')}</MenuItem>
                        </Select>
                    </FormControl>
                    {paymentMethod === 2 && <TextField label={t('invoices.accountNumber')} value={accountNumber} onChange={event => setAccountNumber(event.target.value)} required fullWidth />}
                </DialogContent>
                <DialogActions>
                    <Button disabled={generating} onClick={() => setDocumentInvoice(null)}>{t('opt.cancel')}</Button>
                    <Button disabled={generating || (paymentMethod === 2 && !accountNumber.trim())} onClick={() => void generateDocument()} variant="contained">{generating ? t('invoices.generating') : t('invoices.generateAndDownload')}</Button>
                </DialogActions>
            </Dialog>
        </Grid>
    );
};

export default InvoicesPage;
