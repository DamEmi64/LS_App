using Base;

namespace RPG.Domain.Dictionaries
{
    [Dictionary("RPG file types")]
    public class RPGFileTypes
    {
        public static DictionaryItem Json => EntityDictionary.Item(100401, "Json");
        public static DictionaryItem OldJson => EntityDictionary.Item(100402, "Old json (NOT SUPPORTED)");
        public static DictionaryItem Firebase => EntityDictionary.Item(100403, "Firebase data");
    }
}
