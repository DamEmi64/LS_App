using Base;

namespace Events.Domain.Dictionaries
{
    [Dictionary("Automation events")]
    public class AutomationEvents
    {
        public static DictionaryItem EventCreated => EntityDictionary.Item(30501, "Event created");
        public static DictionaryItem UserSignIn => EntityDictionary.Item(30502, "User sign in to event");
    }
}
