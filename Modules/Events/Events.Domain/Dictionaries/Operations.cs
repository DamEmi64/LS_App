using Base;

namespace Events.Domain.Dictionaries
{
    [Dictionary("Operations")]
    public class Operations
    {
        public static DictionaryItem SendReminder => EntityDictionary.Item(14, "Send reminder about event");
        public static DictionaryItem SendInvitation => EntityDictionary.Item(15, "Send invitation to event");
    }
}
