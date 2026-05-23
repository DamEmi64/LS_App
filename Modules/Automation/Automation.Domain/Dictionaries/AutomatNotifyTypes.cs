using Base;

namespace Automation.Domain.Dictionaries
{
    [Dictionary("Notify types")]
    public class AutomatNotifyTypes
    {
        public static DictionaryItem AutomatCreated => EntityDictionary.Item(1044, "Automat Created", "Automaton was created");
        public static DictionaryItem AutomatUpdated => EntityDictionary.Item(1045, "Automat Updated", "Automaton was updated");
        public static DictionaryItem AutomatDeleted => EntityDictionary.Item(1046, "Automat Deleted", "Automaton was deleted");
        public static DictionaryItem AutomatTurnedOff => EntityDictionary.Item(1047, "Automat Turn off", "Automaton was turned off");
        public static DictionaryItem AutomatTurnedOn => EntityDictionary.Item(1048, "Automat Turn on", "Automaton was turned on");

    }
}
