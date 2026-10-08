using Base;

namespace Automation.Domain.Dictionaries
{
    [Dictionary("Operations")]
    public class Operations
    {
        public static DictionaryItem ExecuteAutomat => EntityDictionary.Item(10601, "Execute Automaton");
        public static DictionaryItem ArchiveData => EntityDictionary.Item(10602, "Archive data");
    }
}
