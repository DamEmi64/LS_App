using Base;

namespace Events.Domain.Dictionaries
{
    [Dictionary("Event categories")]
    public class Categories
    {
        public static DictionaryItem Movies => EntityDictionary.Item(11001, "Movies");
        public static DictionaryItem Concert => EntityDictionary.Item(11002, "Concert");
        public static DictionaryItem Vacation => EntityDictionary.Item(11003, "Vacation");
        public static DictionaryItem Meeting => EntityDictionary.Item(11004, "Meeting");
        public static DictionaryItem Games => EntityDictionary.Item(11005, "Games");
    }
}
