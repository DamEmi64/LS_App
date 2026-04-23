using Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Communication.Domain.Dictionaries
{
    [Dictionary("Fluid functions")]
    public class FluidFunctions
    {
        public static DictionaryItem RandomNumber => EntityDictionary.Item(601, "Random number");
        public static DictionaryItem Random => EntityDictionary.Item(602, "Random");
        public static DictionaryItem RandomUnique => EntityDictionary.Item(603, "Random Unique");
        public static DictionaryItem Increment => EntityDictionary.Item(604, "Increment");
    }
}
