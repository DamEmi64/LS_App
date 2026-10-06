namespace Base
{
    /// <summary>
    ///     Built-in notification dictionary values
    /// </summary>
    [Dictionary("Notify types")]
    public class NotifyTypes
    {
        public static DictionaryItem Log => EntityDictionary.Item(3001, "Log");
        public static DictionaryItem ProcessError => EntityDictionary.Item(3002, "Internal Process Error");
        public static DictionaryItem ProcessStart => EntityDictionary.Item(3003, "Process started");
        public static DictionaryItem ProcessCompleted => EntityDictionary.Item(3004, "Process completed");
        public static DictionaryItem ProcessFailed => EntityDictionary.Item(3005, "Process failed");
        public static DictionaryItem ProcessQueued => EntityDictionary.Item(3006, "Process Queued");
    }
}
