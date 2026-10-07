import { axiosInstance, call } from '@/shared/components/apiClient';
import type { UserData } from '@/features/auth';
import type { Invoice, InvoiceSaveDto } from '../types';

export function getMyInvoices() {
    return call<Invoice[]>(api => api.invoiceApi.getCollector, {}, data => (data ?? []) as Invoice[]);
}

export function getInvoicesToPay() {
    return call<Invoice[]>(api => api.invoiceApi.getRecipient, {}, data => (data ?? []) as Invoice[]);
}

export function markInvoicePositionPaid(invoiceId: string, positionId: string) {
    return call<void, { id: string; positionId: string }>(api => api.invoiceApi.updateByIdPositionsByPositionIdPaid, {
        id: invoiceId,
        positionId,
    });
}

export function loadInvoiceRecipients() {
    return call<{ data: UserData[] }>(api => api.homeApi.getUsers, {});
}

export function createInvoice(invoice: InvoiceSaveDto) {
    return call<void, { saveInvoiceDto: InvoiceSaveDto }>(api => api.invoiceApi.create, {
        saveInvoiceDto: toSaveInvoiceDto(invoice),
    });
}

export function updateInvoice(id: string, invoice: InvoiceSaveDto) {
    return call<void, { id: string; saveInvoiceDto: InvoiceSaveDto }>(api => api.invoiceApi.updateById, {
        id,
        saveInvoiceDto: toSaveInvoiceDto(invoice),
    });
}

export function addInvoicePosition(id: string, position: InvoiceSaveDto['positions'][number]) {
    return axiosInstance.post(`/api/Invoices/${id}/positions`, position);
}

export function deleteInvoice(id: string) {
    return call<void, { id: string }>(api => api.invoiceApi.deleteById, { id });
}

export async function generateInvoiceDocument(id: string, paymentMethod: number, accountNumber?: string) {
    await axiosInstance.post(`/api/Invoices/${id}/document`, {
        paymentMethod,
        accountNumber,
    });

    const deadline = Date.now() + 5 * 60 * 1000;
    while (Date.now() < deadline) {
        await new Promise(resolve => window.setTimeout(resolve, 3000));
        try {
            const response = await axiosInstance.get<Blob>(`/api/Invoices/${id}/document`, { responseType: 'blob' });
            const url = URL.createObjectURL(response.data);
            const link = document.createElement('a');
            link.href = url;
            link.download = 'invoice.pdf';
            link.click();
            window.setTimeout(() => URL.revokeObjectURL(url), 1000);
            return;
        } catch (error) {
            const status = (error as { response?: { status?: number } })?.response?.status;
            if (status !== undefined && status !== 404) throw error;
        }
    }
    throw new Error('Invoice generation timed out.');
}

export async function downloadInvoiceDocument(id: string, title: string) {
    const pdf = await call<Blob, { id: string }>(
        api => request => api.invoiceApi.getByIdDocument(request, { responseType: 'blob' }),
        { id },
        data => data as Blob,
    );
    const url = URL.createObjectURL(pdf);
    const link = document.createElement('a');
    link.href = url;
    link.download = `${title.replace(/[<>:"/\\|?*]/g, '_') || 'invoice'}.pdf`;
    link.click();
    window.setTimeout(() => URL.revokeObjectURL(url), 1000);
}

export function getInvoiceHtml(id: string) {
    return call<string, { id: string }>(
        api => request => api.invoiceApi.getByIdHtml(request, { responseType: 'text' }),
        { id },
        data => String(data),
    );
}

function toSaveInvoiceDto(invoice: InvoiceSaveDto): InvoiceSaveDto {
    return {
        title: invoice.title,
        recipientId: invoice.recipientId,
        positions: invoice.positions.map(position => ({
            title: position.title,
            value: position.value,
            invoiceDate: position.invoiceDate,
        })),
    };
}
