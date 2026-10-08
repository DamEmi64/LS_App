using Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invoices.Domain.Entities
{
    public class InvoicePosition : Entity
    {
        public required string Title { get; set; }
        public required decimal Value { get; set; }
        public DateTime InvoiceDate { get; set; } = DateTime.Today;
        public int Status { get; set; }
    }
}
