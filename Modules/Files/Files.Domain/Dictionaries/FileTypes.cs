using Base;

namespace Files.Domain.Dictionaries
{
    [Dictionary("File types")]
    public class FileTypes
    {
        public static DictionaryItem Games => EntityDictionary.Item(20201, "Games");
        public static DictionaryItem Documents => EntityDictionary.Item(20202, "Documents");
        public static DictionaryItem Study => EntityDictionary.Item(20203, "Study Files");
    }
}