using Base.Entities;

namespace Communication.Domain.Dictionaries
{
    [Dictionary("Operations")]
    public class Operations
    {
        public static DictionaryItem SendEmail => EntityDictionary.Item(21, "Send email");
        public static DictionaryItem GenerateFromTemplate => EntityDictionary.Item(22, "Generate from template");
    }
}