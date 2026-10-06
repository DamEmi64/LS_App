using Base;

namespace RPG.Domain.Dictionaries
{
    [Dictionary("Operations")]
    public class Operations
    {
        public static DictionaryItem GenerateSummary => EntityDictionary.Item(7, "Generate RPG session summary", "Job for generating RPG session summary");
        public static DictionaryItem SentToFirebase => EntityDictionary.Item(8, "Send RPG data to firebase app", "Job for sending RPG data to firebase app");
        public static DictionaryItem GetLastRPG => EntityDictionary.Item(9, "Get Last RPG data", "Job for getting Last RPG data");
        public static DictionaryItem GenerateStoryFromSummary => EntityDictionary.Item(10, "Generate story from summary", "Job for generating story from summary");
        public static DictionaryItem ImportRPGFromFile => EntityDictionary.Item(11, "Import PRG from file", "Job for importing RPG from file");
    }
}