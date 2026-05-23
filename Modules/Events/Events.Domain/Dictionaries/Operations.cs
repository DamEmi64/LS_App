using Base;

namespace Events.Domain.Dictionaries
{
    public class Operations
    {
        public static DictionaryItem SendReminder => EntityDictionary.Item(51, "Send reminder about event");
        public static DictionaryItem SendInvitation => EntityDictionary.Item(52, "Send invitation to event");
    }
}
