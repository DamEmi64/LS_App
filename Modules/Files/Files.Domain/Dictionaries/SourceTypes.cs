using Base;

namespace Files.Domain.Dictionaries
{
    [Dictionary("File source type")]
    public class SourceTypes
    {
        public static DictionaryItem Local => EntityDictionary.Item(60201, "Local");
        public static DictionaryItem FuckingFast => EntityDictionary.Item(60202, "FuckingFast website");
    }
}