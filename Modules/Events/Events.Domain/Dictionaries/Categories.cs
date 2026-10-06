using Base;

namespace Events.Domain.Dictionaries
{
    [Dictionary("Event categories")]
    public class Categories
    {
        public static DictionaryItem Movies => EntityDictionary.Item(120501, "Movies");
        public static DictionaryItem Concert => EntityDictionary.Item(120502, "Concert");
        public static DictionaryItem Vacation => EntityDictionary.Item(120503, "Vacation");
        public static DictionaryItem Meeting => EntityDictionary.Item(120504, "Meeting");
        public static DictionaryItem Games => EntityDictionary.Item(120505, "Games");
    }
}
