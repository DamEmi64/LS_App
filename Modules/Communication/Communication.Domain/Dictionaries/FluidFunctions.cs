using Base;

namespace Communication.Domain.Dictionaries
{
    [Dictionary("Fluid functions")]
    public class FluidFunctions
    {
        public static DictionaryItem RandomNumber => EntityDictionary.Item(80301, "Random number", "RandomNumber()");
        public static DictionaryItem Random => EntityDictionary.Item(80302, "Random", "Random");
        public static DictionaryItem RandomUnique => EntityDictionary.Item(80303, "Random Unique", "RandomUnique");
        public static DictionaryItem Increment => EntityDictionary.Item(80304, "Increment", "Increment()");
    }
}
