using Base;

namespace Files.Domain.Dictionaries
{
    [Dictionary("File types")]
    public class FileTypes
    {
        public static DictionaryItem Games => EntityDictionary.Item(1001, "Games");
        public static DictionaryItem Documents => EntityDictionary.Item(1002, "Documents");
        public static DictionaryItem Study => EntityDictionary.Item(1003, "Study Files");
    }
}