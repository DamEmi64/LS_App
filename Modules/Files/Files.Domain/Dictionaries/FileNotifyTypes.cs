using Base;

namespace Files.Domain.Dictionaries
{
    [Dictionary("Notify types")]
    public class FileNotifyTypes
    {
        public static DictionaryItem FileSave => EntityDictionary.Item(3007, "File saved");
        public static DictionaryItem FileDeleted => EntityDictionary.Item(3008, "File deleted");
        public static DictionaryItem FileUpdated => EntityDictionary.Item(3009, "File updated");
        public static DictionaryItem FileNotFound => EntityDictionary.Item(3010, "File not found");
        public static DictionaryItem FileAlreadyExists => EntityDictionary.Item(3011, "File already exists");
        public static DictionaryItem FileNotSaved => EntityDictionary.Item(3012, "File not saved");
    }
}
