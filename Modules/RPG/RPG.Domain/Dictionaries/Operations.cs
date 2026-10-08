using Base;

namespace RPG.Domain.Dictionaries
{
    [Dictionary("Operations")]
    public class Operations
    {
        public static DictionaryItem GenerateSummary => EntityDictionary.Item(10401, "Generate RPG session summary", "Job for generating RPG session summary");
        public static DictionaryItem SentToFirebase => EntityDictionary.Item(10402, "Send RPG data to firebase app", "Job for sending RPG data to firebase app");
        public static DictionaryItem GetLastRPG => EntityDictionary.Item(10403, "Get Last RPG data", "Job for getting Last RPG data");
        public static DictionaryItem GenerateStoryFromSummary => EntityDictionary.Item(10404, "Generate story from summary", "Job for generating story from summary");
        public static DictionaryItem ImportRPGFromFile => EntityDictionary.Item(10405, "Import PRG from file", "Job for importing RPG from file");
    }
}