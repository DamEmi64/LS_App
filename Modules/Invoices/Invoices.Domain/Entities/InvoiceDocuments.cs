using Base;

namespace Invoices.Domain.Entities
{
    public class InvoiceDocument : Entity
    {
        public  required Invoice Invoice { get; set; }
        public string? Html { get; set; }
        public byte[] Pdf { get; set; } = [];
    }
}
