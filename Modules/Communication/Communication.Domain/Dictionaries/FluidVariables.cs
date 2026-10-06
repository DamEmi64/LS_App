using Base;

namespace Communication.Domain.Dictionaries
{
    [Dictionary("Fluid variables")]
    public class FluidVariables
    {
        public static DictionaryItem UserData => EntityDictionary.Item(8001, "User", "User");
        public static DictionaryItem Sender => EntityDictionary.Item(8002, "Sender", "Sender");
        public static DictionaryItem Recipient => EntityDictionary.Item(8003, "To", "To");
        public static DictionaryItem Recipients => EntityDictionary.Item(8004, "Recipients", "Recipients");
        public static DictionaryItem Counter => EntityDictionary.Item(8005, "Counter", "Counter");
    }
}
