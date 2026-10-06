using Base;

namespace RPG.Domain.Dictionaries
{
    [Dictionary("Automation events")]
    public class AutomationEvents
    {
        public static DictionaryItem RPGEdited => EntityDictionary.Item(2005, "RPG Edited");
    }
}
