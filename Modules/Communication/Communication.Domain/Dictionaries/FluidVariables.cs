using Base.Entities;

namespace Communication.Domain.Dictionaries
{
    [Dictionary("Fluid variables")]
    public class FluidVariables
    {
        public static DictionaryItem UserData => EntityDictionary.Item(605, "User");
        public static DictionaryItem Sender => EntityDictionary.Item(606, "Sender");
        public static DictionaryItem Receiver => EntityDictionary.Item(607, "Receiver");
        public static DictionaryItem Receivers => EntityDictionary.Item(608, "Receivers");
        public static DictionaryItem Counter => EntityDictionary.Item(609, "Counter");
    }
}
