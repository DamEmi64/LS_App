using Base;

namespace Events.Domain.Dictionaries
{
    [Dictionary("System automats")]
    public class SystemAutomats
    {
        public static DictionaryItem SentEventInvitations => EntityDictionary.Item(140501, "System Automat - send event invitations");
        public static DictionaryItem SentEventReminders => EntityDictionary.Item(140502, "System Automat - send event reminders");
    }
}
