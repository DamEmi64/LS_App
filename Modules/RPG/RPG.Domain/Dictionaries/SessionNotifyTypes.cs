using Base;

namespace RPG.Domain.Dictionaries
{
    [Dictionary("Notify types")]
    public class SessionNotifyTypes
    {
        public static DictionaryItem SessionSaved => EntityDictionary.Item(40401, "Session saved");
        public static DictionaryItem SessionDeleted => EntityDictionary.Item(40402, "Session deleted");
        public static DictionaryItem SessionUpdated => EntityDictionary.Item(40403, "Session updated");
        public static DictionaryItem SessionNotFound => EntityDictionary.Item(40404, "Session not found");
        public static DictionaryItem ChapterSaved => EntityDictionary.Item(40405, "Chapter saved");
        public static DictionaryItem ChapterDeleted => EntityDictionary.Item(40406, "Chapter deleted");
        public static DictionaryItem ChapterUpdated => EntityDictionary.Item(40407, "Chapter updated");
        public static DictionaryItem ChapterNotFound => EntityDictionary.Item(40408, "Chapter not found");
        public static DictionaryItem HeroSaved => EntityDictionary.Item(40409, "Hero saved");
        public static DictionaryItem HeroDeleted => EntityDictionary.Item(40410, "Hero deleted");
        public static DictionaryItem HeroUpdated => EntityDictionary.Item(40411, "Hero updated");
        public static DictionaryItem HeroNotFound => EntityDictionary.Item(40412, "Hero not found");
        public static DictionaryItem PlaceSaved => EntityDictionary.Item(40413, "Place saved");
        public static DictionaryItem PlaceDeleted => EntityDictionary.Item(40414, "Place deleted");
        public static DictionaryItem PlaceUpdated => EntityDictionary.Item(40415, "Place updated");
        public static DictionaryItem PlaceNotFound => EntityDictionary.Item(40416, "Place not found");
    }
}
