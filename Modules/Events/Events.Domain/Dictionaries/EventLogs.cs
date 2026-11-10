using Base;
namespace Events.Domain.Dictionaries
{
    [Dictionary("Logs")]
    public class EventLogs
    {
        public static DictionaryItem EventCreated => EntityDictionary.Item(40501, "Event Created", "Event was created");
        public static DictionaryItem EventUpdated => EntityDictionary.Item(40502, "Event Updated", "Event was updated");
        public static DictionaryItem EventDeleted => EntityDictionary.Item(40503, "Event Deleted", "Event was deleted");
        public static DictionaryItem EventSignIn => EntityDictionary.Item(40504, "Event user Sign In", "Event user was signed in");
        public static DictionaryItem EventSignOut => EntityDictionary.Item(40505, "Event user Sign Out", "Event user was signed out");
        public static DictionaryItem SendInvitation => EntityDictionary.Item(40506, "Send Invitation", "Event invitation was sent");
        public static DictionaryItem SetReminder => EntityDictionary.Item(40507, "Set reminder", "Event reminder was set");
        public static DictionaryItem RemoveReminder => EntityDictionary.Item(40508, "Remove reminder", "Event reminder was removed");
        public static DictionaryItem ValidationFailed => EntityDictionary.Item(40801, "Validation failed: {0}");
        public static DictionaryItem InvoiceDetailsRequired => EntityDictionary.Item(40802, "Invoice title, position, and at least one allocation are required.");
        public static DictionaryItem InvalidAllocation => EntityDictionary.Item(40803, "Allocations must have a user and a non-negative value.");
        public static DictionaryItem DuplicateAllocation => EntityDictionary.Item(40804, "A user can have only one allocation per position.");
        public static DictionaryItem AllocationNotParticipant => EntityDictionary.Item(40805, "Every allocation must belong to an event participant.");
        public static DictionaryItem UserNotFound => EntityDictionary.Item(40806, "User '{0}' was not found.");

    }
}
