using Invoices.Infrastructure.Models;

namespace Invoices.Application.Dtos
{
    public sealed class GenerateInvoiceDocumentDto
    {
        public InvoicePaymentMethod PaymentMethod { get; set; }
        public string? AccountNumber { get; set; }
    }
}
