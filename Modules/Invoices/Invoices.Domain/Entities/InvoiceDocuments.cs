using Base;

namespace Invoices.Domain.Entities
{
    public class InvoiceDocument : Entity
    {
        public string? Html { get; set; }
        public byte[] Pdf { get; set; } = [];
    }
}
