using Base.Entities;

namespace Files.Domain.Dictionaries
{
    [Dictionary("File source type")]
    public class SourceTypes
    {
        public static DictionaryItem Local => EntityDictionary.Item(300, "Local");
        public static DictionaryItem FuckingFast => EntityDictionary.Item(301, "FuckingFast website");
    }
}