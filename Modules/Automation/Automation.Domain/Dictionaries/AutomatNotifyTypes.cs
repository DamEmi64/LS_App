using Base;

namespace Automation.Domain.Dictionaries
{
    [Dictionary("Notify types")]
    public class AutomatNotifyTypes
    {
        public static DictionaryItem AutomatCreated => EntityDictionary.Item(40601, "Automat Created", "Automaton was created");
        public static DictionaryItem AutomatUpdated => EntityDictionary.Item(40602, "Automat Updated", "Automaton was updated");
        public static DictionaryItem AutomatDeleted => EntityDictionary.Item(40603, "Automat Deleted", "Automaton was deleted");
        public static DictionaryItem AutomatTurnedOff => EntityDictionary.Item(40604, "Automat Turn off", "Automaton was turned off");
        public static DictionaryItem AutomatTurnedOn => EntityDictionary.Item(40605, "Automat Turn on", "Automaton was turned on");

    }
}
