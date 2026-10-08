export interface InvoicePosition {
    id: string;
    title: string;
    value: number;
    invoiceDate: string;
    status: number;
}

export interface Invoice {
    id: string;
    title: string;
    collectorId: string;
    collectorLogin: string;
    recipientId: string;
    recipientLogin: string;
    positions: InvoicePosition[];
}

export interface InvoiceSaveDto {
    title: string;
    recipientId: string;
    positions: { title: string; value: number; invoiceDate: string }[];
}

export const INVOICE_STATUS = {
    unpaid: 130801,
    sent: 130802,
    paid: 130803,
} as const;
