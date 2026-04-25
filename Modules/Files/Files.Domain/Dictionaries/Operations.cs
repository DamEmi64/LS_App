using Base;

namespace Files.Domain.Dictionaries
{
    [Dictionary("Operations")]
    public class Operations
    {
        public static DictionaryItem ImportFile => EntityDictionary.Item(11, "Import file");
        public static DictionaryItem MoveFile => EntityDictionary.Item(12, "Move file");
        public static DictionaryItem CopyFile => EntityDictionary.Item(13, "Copy file");
        public static DictionaryItem DeleteFile => EntityDictionary.Item(14, "Delete file");
    }
}