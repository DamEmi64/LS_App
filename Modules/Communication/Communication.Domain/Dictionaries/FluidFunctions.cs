using Base;

namespace Communication.Domain.Dictionaries
{
    [Dictionary("Fluid functions")]
    public class FluidFunctions
    {
        public static DictionaryItem RandomNumber => EntityDictionary.Item(7001, "Random number", "RandomNumber()");
        public static DictionaryItem Random => EntityDictionary.Item(7002, "Random", "Random");
        public static DictionaryItem RandomUnique => EntityDictionary.Item(7003, "Random Unique", "RandomUnique");
        public static DictionaryItem Increment => EntityDictionary.Item(7004, "Increment", "Increment()");
    }
}
