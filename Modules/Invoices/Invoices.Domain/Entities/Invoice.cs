using Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invoices.Domain.Entities
{
    public class Invoice : Entity
    {
        public required string Title { get; set; }
        public required UserData Collector { get; set; }
        public required UserData Recipient { get; set; }
        public List<InvoicePosition> Positions { get; set; } = new();
    }
}
