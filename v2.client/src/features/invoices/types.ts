export interface InvoicePosition {
    id: string;
    title: string;
    value: number;
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
    positions: { title: string; value: number }[];
}

export const INVOICE_STATUS = {
    unpaid: 501,
    paid: 503,
} as const;
