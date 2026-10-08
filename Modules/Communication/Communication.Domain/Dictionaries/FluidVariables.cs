using Base;

namespace Communication.Domain.Dictionaries
{
    [Dictionary("Fluid variables")]
    public class FluidVariables
    {
        public static DictionaryItem UserData => EntityDictionary.Item(90301, "User", "User");
        public static DictionaryItem Sender => EntityDictionary.Item(90302, "Sender", "Sender");
        public static DictionaryItem Recipient => EntityDictionary.Item(90303, "To", "To");
        public static DictionaryItem Recipients => EntityDictionary.Item(90304, "Recipients", "Recipients");
        public static DictionaryItem Counter => EntityDictionary.Item(90305, "Counter", "Counter");
    }
}
