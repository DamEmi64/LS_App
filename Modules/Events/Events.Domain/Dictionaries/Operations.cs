using Base;

namespace Events.Domain.Dictionaries
{
    [Dictionary("Operations")]
    public class Operations
    {
        public static DictionaryItem SendReminder => EntityDictionary.Item(10501, "Send reminder about event");
        public static DictionaryItem SendInvitation => EntityDictionary.Item(10502, "Send invitation to event");
    }
}
