using Base;
using Invoices.Domain.Entities;
using Invoices.Infrastructure.Models;

namespace Invoices.Infrastructure.Services.InvoiceDocumentService
{
    public interface IInvoiceDocumentService
    {
        Task GenerateInvoice(Guid invoiceId, InvoicePaymentMethod paymentMethod, UserData collector, string? accountNo = null);
        Task GenerateAndSendInvoice(Guid invoiceId, InvoicePaymentMethod paymentMethod, UserData collector, string? accountNo = null);
    }
}
