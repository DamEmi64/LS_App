using Base;

namespace Files.Domain.Dictionaries
{
    [Dictionary("Operations")]
    public class Operations
    {
        public static DictionaryItem ImportFile => EntityDictionary.Item(1, "Import file");
        public static DictionaryItem MoveFile => EntityDictionary.Item(2, "Move file");
        public static DictionaryItem CopyFile => EntityDictionary.Item(3, "Copy file");
        public static DictionaryItem DeleteFile => EntityDictionary.Item(4, "Delete file");
    }
}