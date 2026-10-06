using Base;

namespace RPG.Domain.Dictionaries
{
    [Dictionary("Notify types")]
    public class SessionNotifyTypes
    {
        public static DictionaryItem SessionSaved => EntityDictionary.Item(3013, "Session saved");
        public static DictionaryItem SessionDeleted => EntityDictionary.Item(3014, "Session deleted");
        public static DictionaryItem SessionUpdated => EntityDictionary.Item(3015, "Session updated");
        public static DictionaryItem SessionNotFound => EntityDictionary.Item(3016, "Session not found");
        public static DictionaryItem ChapterSaved => EntityDictionary.Item(3017, "Chapter saved");
        public static DictionaryItem ChapterDeleted => EntityDictionary.Item(3018, "Chapter deleted");
        public static DictionaryItem ChapterUpdated => EntityDictionary.Item(3019, "Chapter updated");
        public static DictionaryItem ChapterNotFound => EntityDictionary.Item(3020, "Chapter not found");
        public static DictionaryItem HeroSaved => EntityDictionary.Item(3021, "Hero saved");
        public static DictionaryItem HeroDeleted => EntityDictionary.Item(3022, "Hero deleted");
        public static DictionaryItem HeroUpdated => EntityDictionary.Item(3023, "Hero updated");
        public static DictionaryItem HeroNotFound => EntityDictionary.Item(3024, "Hero not found");
        public static DictionaryItem PlaceSaved => EntityDictionary.Item(3025, "Place saved");
        public static DictionaryItem PlaceDeleted => EntityDictionary.Item(3026, "Place deleted");
        public static DictionaryItem PlaceUpdated => EntityDictionary.Item(3027, "Place updated");
        public static DictionaryItem PlaceNotFound => EntityDictionary.Item(3028, "Place not found");
    }
}
