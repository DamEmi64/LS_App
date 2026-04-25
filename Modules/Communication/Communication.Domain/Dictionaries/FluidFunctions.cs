using Base;

namespace Communication.Domain.Dictionaries
{
    [Dictionary("Fluid functions")]
    public class FluidFunctions
    {
        public static DictionaryItem RandomNumber => EntityDictionary.Item(601, "Random number", "RandomNumber()");
        public static DictionaryItem Random => EntityDictionary.Item(602, "Random", "Random");
        public static DictionaryItem RandomUnique => EntityDictionary.Item(603, "Random Unique", "RandomUnique");
        public static DictionaryItem Increment => EntityDictionary.Item(604, "Increment", "Increment()");
    }
}
