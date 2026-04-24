using Base;

namespace RPG.Domain.Dictionaries
{
    [Dictionary("Notify types")]
    public class SessionNotifyTypes
    {
        public static DictionaryItem SessionSaved => EntityDictionary.Item(1012, "Session saved");
        public static DictionaryItem SessionDeleted => EntityDictionary.Item(1013, "Session deleted");
        public static DictionaryItem SessionUpdated => EntityDictionary.Item(1014, "Session updated");
        public static DictionaryItem SessionNotFound => EntityDictionary.Item(1015, "Session not found");
        public static DictionaryItem ChapterSaved => EntityDictionary.Item(1016, "Chapter saved");
        public static DictionaryItem ChapterDeleted => EntityDictionary.Item(1017, "Chapter deleted");
        public static DictionaryItem ChapterUpdated => EntityDictionary.Item(1018, "Chapter updated");
        public static DictionaryItem ChapterNotFound => EntityDictionary.Item(1019, "Chapter not found");
        public static DictionaryItem HeroSaved => EntityDictionary.Item(1020, "Hero saved");
        public static DictionaryItem HeroDeleted => EntityDictionary.Item(1021, "Hero deleted");
        public static DictionaryItem HeroUpdated => EntityDictionary.Item(1022, "Hero updated");
        public static DictionaryItem HeroNotFound => EntityDictionary.Item(1023, "Hero not found");
        public static DictionaryItem PlaceSaved => EntityDictionary.Item(1024, "Place saved");
        public static DictionaryItem PlaceDeleted => EntityDictionary.Item(1025, "Place deleted");
        public static DictionaryItem PlaceUpdated => EntityDictionary.Item(1026, "Place updated");
        public static DictionaryItem PlaceNotFound => EntityDictionary.Item(1027, "Place not found");
    }
}
