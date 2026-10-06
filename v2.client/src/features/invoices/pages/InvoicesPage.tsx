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
} from '@mui/material';
import { useTranslation } from 'react-i18next';
import OperationCell from '@/shared/components/operationCell';
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
import { createInvoice, deleteInvoice, getInvoicesToPay, getMyInvoices, markInvoicePositionPaid, updateInvoice } from '../services/invoiceService';
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

    const invoiceOperations: Operations<InvoiceRow>[] = mode === 'mine' ? [
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
        </Grid>
    );
};

export default InvoicesPage;
