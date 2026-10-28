using Base;

namespace Invoices.Domain.Dictionaries
{
    [Dictionary("Logs")]
    public class InvoiceLogs
    {
        public static DictionaryItem RecipientNotFound => EntityDictionary.Item(40901, "Recipient was not found.");
        public static DictionaryItem PositionInvalid => EntityDictionary.Item(40902, "Position title and a non-negative value are required.");
        public static DictionaryItem AccountNumberRequired => EntityDictionary.Item(40903, "Account number is required.");
    }
}

