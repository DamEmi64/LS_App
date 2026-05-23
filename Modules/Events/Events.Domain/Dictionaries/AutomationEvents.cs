using Base;

namespace Events.Domain.Dictionaries
{
    [Dictionary("Automation events")]
    public class AutomationEvents
    {
        public static DictionaryItem EventCreated => EntityDictionary.Item(112, "Event created");
        public static DictionaryItem UserSignIn => EntityDictionary.Item(113, "User sign in to event");
    }
}
