using Base;

namespace Communication.Domain.Dictionaries
{
    [Dictionary("Fluid variables")]
    public class FluidVariables
    {
        public static DictionaryItem UserData => EntityDictionary.Item(605, "User", "User");
        public static DictionaryItem Sender => EntityDictionary.Item(606, "Sender", "Sender");
        public static DictionaryItem Recipient => EntityDictionary.Item(607, "Recipient", "Recipient");
        public static DictionaryItem Recipients => EntityDictionary.Item(608, "Recipients", "Recipients");
        public static DictionaryItem Counter => EntityDictionary.Item(609, "Counter", "Counter");
    }
}
