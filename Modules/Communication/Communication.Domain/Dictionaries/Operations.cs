using Base;

namespace Communication.Domain.Dictionaries
{
    [Dictionary("Operations")]
    public class Operations
    {
        public static DictionaryItem SendEmail => EntityDictionary.Item(5, "Send email");
        public static DictionaryItem GenerateFromTemplate => EntityDictionary.Item(6, "Generate from template");
    }
}