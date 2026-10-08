import { call } from '@/shared/components/apiClient';
import type { EventDto } from '@/shared/api/generated';
import { loadEvents } from '@/features/events/services/eventService';

export type EventInvoiceDraft = {
    eventId: string;
    title: string;
    positions: { positionTitle: string; allocations: { userId: string; value: number }[] }[];
};
export type EventInvoiceRow = { eventId?: string; eventTitle?: string; eventDate?: string; invoiceId?: string; userId?: string; userLogin?: string; value?: number };

export async function loadEventsForCosts(): Promise<EventDto[]> {
    return loadEvents({});
}

export function loadEventInvoices(): Promise<EventInvoiceRow[]> {
    return call<EventInvoiceRow[]>(api => api.eventInvoiceApi.get, {});
}

export async function createEventInvoices(draft: EventInvoiceDraft) {
    await call(api => api.eventInvoiceApi.create, {createEventInvoiceDto:draft});
}
