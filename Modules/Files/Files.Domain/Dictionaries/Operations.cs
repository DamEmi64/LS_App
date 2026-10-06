using Base;

namespace Files.Domain.Dictionaries
{
    [Dictionary("Operations")]
    public class Operations
    {
        public static DictionaryItem ImportFile => EntityDictionary.Item(10201, "Import file");
        public static DictionaryItem MoveFile => EntityDictionary.Item(10202, "Move file");
        public static DictionaryItem CopyFile => EntityDictionary.Item(10203, "Copy file");
        public static DictionaryItem DeleteFile => EntityDictionary.Item(10204, "Delete file");
    }
}