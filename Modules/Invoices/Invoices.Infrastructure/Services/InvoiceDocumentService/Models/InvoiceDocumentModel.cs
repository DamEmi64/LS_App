using Invoices.Domain.Entities;

namespace Invoices.Infrastructure.Models
{
    public enum InvoicePaymentMethod
    {
        None = 0,
        BlikPhone = 1,
        AccountNumber = 2
    }

    /// <summary>Data rendered into an invoice document.</summary>
    public sealed class InvoiceDocumentModel
    {
        public required Invoice Invoice { get; init; }
        public Guid InvoiceId => Invoice.Id;
        public InvoicePaymentMethod PaymentMethod { get; init; }
        public string? AccountNumber { get; init; }
        /// <summary>Collector's phone from user data, used for BLIK payments.</summary>
        public string? CollectorPhoneNumber { get; init; }
    }
}
