using Base.Entities;

namespace Automation.Domain.Dictionaries
{
    [Dictionary("Operations")]
    public class Operations
    {
        public static DictionaryItem ExecuteAutomat => EntityDictionary.Item(40, "Execute Automaton");
        public static DictionaryItem ArchiveData => EntityDictionary.Item(41, "Archive data");
    }
}
