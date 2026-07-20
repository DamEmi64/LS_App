using Base;

namespace Events.Domain.Dictionaries
{
    [Dictionary("Event categories")]
    public class Categories
    {
        public static DictionaryItem Movies => EntityDictionary.Item(421, "Movies");
        public static DictionaryItem Concert => EntityDictionary.Item(422, "Concert");
        public static DictionaryItem Vacation => EntityDictionary.Item(423, "Vacation");
        public static DictionaryItem Meeting => EntityDictionary.Item(424, "Meeting");
        public static DictionaryItem Games => EntityDictionary.Item(425, "Games");
    }
}
