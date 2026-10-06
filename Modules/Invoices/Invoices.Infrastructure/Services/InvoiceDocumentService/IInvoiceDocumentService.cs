using Invoices.Domain.Entities;

namespace Invoices.Infrastructure.Services.InvoiceDocumentService
{
    public interface IInvoiceDocumentService
    {
        Task<string> GenerateHtml(Invoice invoice);
        Task<byte[]> GeneratePdf(Invoice invoice);
    }
}
