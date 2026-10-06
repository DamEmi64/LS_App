using Base;

namespace Communication.Domain.Dictionaries
{
    [Dictionary("Email statuses")]
    public class EmailStatus
    {
        public static DictionaryItem Created => EntityDictionary.Item(6001, "Created", "Created");
        public static DictionaryItem Sent => EntityDictionary.Item(6002, "Sent", "Sent");
        public static DictionaryItem SentConfirmed => EntityDictionary.Item(6003, "Sent Confirmed", "Sent Confirmed");
        public static DictionaryItem Open => EntityDictionary.Item(6004, "Open", "Open");
        public static DictionaryItem Rejected => EntityDictionary.Item(6005, "Rejected", "Rejected");
    }
}
