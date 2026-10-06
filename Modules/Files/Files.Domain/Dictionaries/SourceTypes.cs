using Base;

namespace Files.Domain.Dictionaries
{
    [Dictionary("File source type")]
    public class SourceTypes
    {
        public static DictionaryItem Local => EntityDictionary.Item(5001, "Local");
        public static DictionaryItem FuckingFast => EntityDictionary.Item(5002, "FuckingFast website");
    }
}