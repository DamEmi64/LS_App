using Base;

namespace AutomationBase.Dictionaries
{
    [Dictionary("Automation events")]
    public class AutomationEvents
    {
        public static DictionaryItem Cron => EntityDictionary.Item(110, "Cron", "Event triggered by cron expression");
    }
}
