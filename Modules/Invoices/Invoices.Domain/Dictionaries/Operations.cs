using Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invoices.Domain.Dictionaries
{
    [Dictionary("Operations")]
    public class Operations
    {
        public static DictionaryItem GenerateInvoice => EntityDictionary.Item(16, "Generate Invoice", "Job for generating an invoice");
        public static DictionaryItem SendInvoice => EntityDictionary.Item(17, "Send Invoice", "Job for sending an invoice");
    }
}
