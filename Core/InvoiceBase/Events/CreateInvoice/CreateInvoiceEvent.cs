using Base;
using Base.Connect;

namespace InvoiceBase.Events
{
    public record CreateInvoiceEvent(string Title, UserData Collector, UserData Recipient, List<CreateinvoicePosition> Positions) : IEvent<CreateInvoiceResponse>;
}
