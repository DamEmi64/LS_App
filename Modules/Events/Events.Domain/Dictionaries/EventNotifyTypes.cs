using Base;
namespace Events.Domain.Dictionaries
{
    [Dictionary("Notify types")]
    public class EventNotifyTypes
    {
        public static DictionaryItem EventCreated => EntityDictionary.Item(3048, "Event Created", "Event was created");
        public static DictionaryItem EventUpdated => EntityDictionary.Item(3049, "Event Updated", "Event was updated");
        public static DictionaryItem EventDeleted => EntityDictionary.Item(3050, "Event Deleted", "Event was deleted");
        public static DictionaryItem EventSignIn => EntityDictionary.Item(3051, "Event user Sign In", "Event user was signed in");
        public static DictionaryItem EventSignOut => EntityDictionary.Item(3052, "Event user Sign Out", "Event user was signed out");
        public static DictionaryItem SendInvitation => EntityDictionary.Item(3053, "Send Invitation", "Event invitation was sent");
        public static DictionaryItem SetReminder => EntityDictionary.Item(3054, "Set reminder", "Event reminder was set");
        public static DictionaryItem RemoveReminder => EntityDictionary.Item(3055, "Remove reminder", "Event reminder was removed");

    }
}
