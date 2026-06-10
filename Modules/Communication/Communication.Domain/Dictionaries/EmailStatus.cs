using Base;

namespace Communication.Domain.Dictionaries
{
    [Dictionary("Email statuses")]
    public class EmailStatus
    {
        public static DictionaryItem Created => EntityDictionary.Item(351, "Created", "Created");
        public static DictionaryItem Sent => EntityDictionary.Item(352, "Sent", "Sent");
        public static DictionaryItem SentConfirmed => EntityDictionary.Item(353, "Sent Confirmed", "Sent Confirmed");
        public static DictionaryItem Open => EntityDictionary.Item(354, "Open", "Open");
        public static DictionaryItem Rejected => EntityDictionary.Item(355, "Rejected", "Rejected");
    }
}
