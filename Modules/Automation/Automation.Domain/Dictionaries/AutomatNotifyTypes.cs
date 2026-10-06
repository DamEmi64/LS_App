using Base;

namespace Automation.Domain.Dictionaries
{
    [Dictionary("Notify types")]
    public class AutomatNotifyTypes
    {
        public static DictionaryItem AutomatCreated => EntityDictionary.Item(3043, "Automat Created", "Automaton was created");
        public static DictionaryItem AutomatUpdated => EntityDictionary.Item(3044, "Automat Updated", "Automaton was updated");
        public static DictionaryItem AutomatDeleted => EntityDictionary.Item(3045, "Automat Deleted", "Automaton was deleted");
        public static DictionaryItem AutomatTurnedOff => EntityDictionary.Item(3046, "Automat Turn off", "Automaton was turned off");
        public static DictionaryItem AutomatTurnedOn => EntityDictionary.Item(3047, "Automat Turn on", "Automaton was turned on");

    }
}
