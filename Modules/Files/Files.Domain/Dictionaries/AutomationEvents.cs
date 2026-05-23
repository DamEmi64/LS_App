using Base;

namespace Files.Domain.Dictionaries
{
    [Dictionary("Automation events")]
    public class AutomationEvents
    {
        public static DictionaryItem FileSaved => EntityDictionary.Item(115, "File saved");
        public static DictionaryItem FileDownloaded => EntityDictionary.Item(111, "File downloaded");
    }
}
