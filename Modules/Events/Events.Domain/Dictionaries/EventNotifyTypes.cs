using Base;
namespace Events.Domain.Dictionaries
{
    [Dictionary("Notify types")]
    public class EventNotifyTypes
    {
        public static DictionaryItem EventCreated = EntityDictionary.Item(1045, "Event Created", "Event was created");
        public static DictionaryItem EventUpdated = EntityDictionary.Item(1046, "Event Updated", "Event was updated");
        public static DictionaryItem EventDeleted = EntityDictionary.Item(1047, "Event Deleted", "Event was deleted");
        public static DictionaryItem EventSignIn = EntityDictionary.Item(1049, "Event user Sign In", "Event user was signed in");
        public static DictionaryItem EventSignOut = EntityDictionary.Item(1050, "Event user Sign Out", "Event user was signed out");
        public static DictionaryItem SendInvitation = EntityDictionary.Item(1051, "Send Invitation", "Event invitation was sent");
        public static DictionaryItem SetReminder = EntityDictionary.Item(1052, "Set reminder", "Event reminder was set");
        public static DictionaryItem RemoveReminder = EntityDictionary.Item(1053, "Remove reminder", "Event reminder was removed");

    }
}
