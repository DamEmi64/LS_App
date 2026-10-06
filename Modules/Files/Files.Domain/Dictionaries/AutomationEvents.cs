using Base;

namespace Files.Domain.Dictionaries
{
    [Dictionary("Automation events")]
    public class AutomationEvents
    {
        public static DictionaryItem FileSaved => EntityDictionary.Item(2006, "File saved");
        public static DictionaryItem FileDownloaded => EntityDictionary.Item(2002, "File downloaded");
    }
}
