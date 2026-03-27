using Base.Entities;

namespace Files.Domain.Dictionaries
{
    [Dictionary("File types")]
    public class FileTypes
    {
        public static DictionaryItem Games => EntityDictionary.Item(101, "Games");
        public static DictionaryItem Documents => EntityDictionary.Item(102, "Documents");
        public static DictionaryItem Study => EntityDictionary.Item(103, "Study Files");
    }
}