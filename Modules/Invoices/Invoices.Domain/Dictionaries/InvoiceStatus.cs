using Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invoices.Domain.Dictionaries
{
    [Dictionary("Invoice Statuses")]
    public class InvoiceStatus
    {
        //TODO Check and change keys
        public static DictionaryItem Unpaid => EntityDictionary.Item(130801, "Unpaid invoice");
        public static DictionaryItem Sent => EntityDictionary.Item(130802, "Sent invoice");
        public static DictionaryItem Paid => EntityDictionary.Item(130803, "Paid invoice");
    }
}
