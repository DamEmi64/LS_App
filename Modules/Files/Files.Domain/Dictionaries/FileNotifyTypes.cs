using Base.Entities;

namespace Files.Domain.Dictionaries
{
    [Dictionary("NotifyTypes")]
    public class FileNotifyTypes
    {
        public static DictionaryItem FileSave => EntityDictionary.Item(1006, "File saved");
        public static DictionaryItem FileDeleted => EntityDictionary.Item(1007, "File deleted");
        public static DictionaryItem FileUpdated => EntityDictionary.Item(1008, "File updated");
        public static DictionaryItem FileNotFound => EntityDictionary.Item(1009, "File not found");
        public static DictionaryItem FileAlreadyExists => EntityDictionary.Item(1010, "File already exists");
        public static DictionaryItem FileNotSaved => EntityDictionary.Item(1011, "File not saved");
    }
}
