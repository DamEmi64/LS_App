using Base;

namespace FilesV2.Domain.Dictionaries
{
    [Dictionary("Logs")]
    public class FileV2Logs
    {
        public static DictionaryItem ZipRequired => EntityDictionary.Item(41001, "A non-empty ZIP file is required.");
        public static DictionaryItem ZipInvalidPath => EntityDictionary.Item(41002, "ZIP contains an invalid path.");
        public static DictionaryItem ZipInvalid => EntityDictionary.Item(41003, "The uploaded file is not a valid ZIP archive.");
        public static DictionaryItem DirectorySelfParent => EntityDictionary.Item(41004, "A directory cannot be its own parent.");
        public static DictionaryItem ParentNotFound => EntityDictionary.Item(41005, "Parent directory not found.");
        public static DictionaryItem DirectoryNotEmpty => EntityDictionary.Item(41006, "Directory is not empty.");
        public static DictionaryItem DirectoryNotFound => EntityDictionary.Item(41007, "Directory not found.");
    }
}

