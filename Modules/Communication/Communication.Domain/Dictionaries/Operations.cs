using Base;

namespace Communication.Domain.Dictionaries
{
    [Dictionary("Operations")]
    public class Operations
    {
        public static DictionaryItem SendEmail => EntityDictionary.Item(10301, "Send email");
        public static DictionaryItem GenerateFromTemplate => EntityDictionary.Item(10302, "Generate from template");
    }
}