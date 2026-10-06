using Base;

namespace RPG.Domain.Dictionaries
{
    [Dictionary("RPG file types")]
    public class RPGFileTypes
    {
        public static DictionaryItem Json => EntityDictionary.Item(9001, "Json");
        public static DictionaryItem OldJson => EntityDictionary.Item(9002, "Old json (NOT SUPPORTED)");
        public static DictionaryItem Firebase => EntityDictionary.Item(9003, "Firebase data");
    }
}
