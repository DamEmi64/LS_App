using Base;

namespace Communication.Domain.Dictionaries
{
    [Dictionary("Email statuses")]
    public class EmailStatus
    {
        public static DictionaryItem Created => EntityDictionary.Item(70301, "Created", "Created");
        public static DictionaryItem Sent => EntityDictionary.Item(70302, "Sent", "Sent");
        public static DictionaryItem SentConfirmed => EntityDictionary.Item(70303, "Sent Confirmed", "Sent Confirmed");
        public static DictionaryItem Open => EntityDictionary.Item(70304, "Open", "Open");
        public static DictionaryItem Rejected => EntityDictionary.Item(70305, "Rejected", "Rejected");
    }
}
