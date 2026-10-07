using Base;
using InvoiceBase.Events;

namespace InvoiceBase
{
    public static class Extensions
    {
        public static Task CreateInvoice(this IConnect connect, string title, UserData collector, UserData recipient, List<CreateinvoicePosition> positions)
            => connect.Send<CreateInvoiceEvent, CreateInvoiceResponse>(new CreateInvoiceEvent(title, collector, recipient, positions));
    }
}
