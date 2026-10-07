namespace Base
{
    /// <summary>
    ///     Built-in notification dictionary values
    /// </summary>
    [Dictionary("Notify types")]
    public class NotifyTypes
    {
        public static DictionaryItem Log => EntityDictionary.Item(40101, "Log");
        public static DictionaryItem ProcessError => EntityDictionary.Item(40102, "Internal Process Error");
        public static DictionaryItem ProcessStart => EntityDictionary.Item(40103, "Process started");
        public static DictionaryItem ProcessCompleted => EntityDictionary.Item(40104, "Process completed");
        public static DictionaryItem ProcessFailed => EntityDictionary.Item(40105, "Process failed");
        public static DictionaryItem ProcessQueued => EntityDictionary.Item(40106, "Process Queued");
    }
}
