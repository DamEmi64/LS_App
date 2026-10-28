using Base;

namespace Invoices.Domain.Dictionaries
{
    [Dictionary("System automats")]
    public class InvoiceAutomats
    {
        public static DictionaryItem UpsertInvoiceDocument => EntityDictionary.Item(140901, "Update or create invoice document");
    }
}
