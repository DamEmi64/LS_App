using Base;

namespace Invoices.Domain.Events
{
    public record InvoiceCreateOrUpdateEvent(Guid InvoiceId) : NotifyEvent;
}
