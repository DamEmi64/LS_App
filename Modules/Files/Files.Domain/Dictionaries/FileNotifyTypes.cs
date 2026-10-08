using Base;

namespace Files.Domain.Dictionaries
{
    [Dictionary("Notify types")]
    public class FileNotifyTypes
    {
        public static DictionaryItem FileSave => EntityDictionary.Item(40201, "File saved");
        public static DictionaryItem FileDeleted => EntityDictionary.Item(40202, "File deleted");
        public static DictionaryItem FileUpdated => EntityDictionary.Item(40203, "File updated");
        public static DictionaryItem FileNotFound => EntityDictionary.Item(40204, "File not found");
        public static DictionaryItem FileAlreadyExists => EntityDictionary.Item(40205, "File already exists");
        public static DictionaryItem FileNotSaved => EntityDictionary.Item(40206, "File not saved");
    }
}
