using Base;

namespace Base
{
    [Dictionary("Notify types")]
    public class NotifyTypes
    {
        public static DictionaryItem Log => EntityDictionary.Item(1000, "Log");
        public static DictionaryItem ProcessError => EntityDictionary.Item(1001, "Internal Process Error");
        public static DictionaryItem ProcessStart => EntityDictionary.Item(1002, "Process started");
        public static DictionaryItem ProcessCompleted => EntityDictionary.Item(1003, "Process completed");
        public static DictionaryItem ProcessFailed => EntityDictionary.Item(1004, "Process failed");
        public static DictionaryItem ProcessQueued => EntityDictionary.Item(1005, "Process Queued");
    }
}