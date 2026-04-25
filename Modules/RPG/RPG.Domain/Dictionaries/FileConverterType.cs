using Base;

namespace RPG.Domain.Dictionaries
{
    [Dictionary("RPG file types")]
    public class RPGFileTypes
    {
        public static DictionaryItem Json => EntityDictionary.Item(501, "Json");
        public static DictionaryItem OldJson => EntityDictionary.Item(502, "Old json (NOT SUPPORTED)");
        public static DictionaryItem Firebase => EntityDictionary.Item(503, "Firebase data");
    }
}
