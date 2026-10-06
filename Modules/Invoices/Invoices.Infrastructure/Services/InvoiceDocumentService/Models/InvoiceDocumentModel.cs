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

        public void Validate()
        {
            ArgumentNullException.ThrowIfNull(Invoice);
            if (!Enum.IsDefined(PaymentMethod))
                throw new ArgumentOutOfRangeException(nameof(PaymentMethod), "Nieznana metoda płatności.");
            if (PaymentMethod == InvoicePaymentMethod.BlikPhone && string.IsNullOrWhiteSpace(CollectorPhoneNumber))
                throw new ArgumentException("Numer telefonu wystawcy jest wymagany przy płatności BLIK.", nameof(CollectorPhoneNumber));
            if (PaymentMethod == InvoicePaymentMethod.AccountNumber && string.IsNullOrWhiteSpace(AccountNumber))
                throw new ArgumentException("Numer rachunku jest wymagany przy płatności przelewem.", nameof(AccountNumber));
        }
    }
}
