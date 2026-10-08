using Base;

namespace Files.Domain.Dictionaries
{
    [Dictionary("Automation events")]
    public class AutomationEvents
    {
        public static DictionaryItem FileSaved => EntityDictionary.Item(30202, "File saved");
        public static DictionaryItem FileDownloaded => EntityDictionary.Item(30201, "File downloaded");
    }
}
