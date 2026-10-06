using System;
using System.Collections.Generic;
using System.Text;

namespace Invoices.Extras.Resources
{
    public class InvoiceDocument
    {
        public required string Title { get; set; }
        public required string Collector { get; set; }
        public required string Recipient { get; set; }
        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
        public string? PaymentMethod { get; set; }
        public string? Phone { get; set; }
        public string? AccountNo { get; set; }

        public string PaymentInfo
            => PaymentMethod?.ToLower() switch
            {
                "blik" => $"Płatność blikiem na nr telefonu: {Phone}",
                "account" => $"Przelewem na konto: {AccountNo}",
                _ => "Nieustalony, skontaktuj się z Wystawcą"
            };
    }

    public class InvoiceItem
    {
        public required string Title { get; set; }
        public decimal Value { get; set; }
    }
}
