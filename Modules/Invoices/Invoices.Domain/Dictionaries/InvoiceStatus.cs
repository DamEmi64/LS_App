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
        public static DictionaryItem Unpaid => EntityDictionary.Item(12001, "Unpaid invoice");
        public static DictionaryItem Sent => EntityDictionary.Item(12002, "Sent invoice");
        public static DictionaryItem Paid => EntityDictionary.Item(12003, "Paid invoice");
    }
}
