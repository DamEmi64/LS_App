using Base.Entities;

namespace RPG.Domain.Dictionaries
{
    [Dictionary("Operations")]
    public class Operations
    {
        public static DictionaryItem GenerateSummary => EntityDictionary.Item(31, "Generate RPG session summary", "Job for generating RPG session summary");
        public static DictionaryItem SentToFirebase => EntityDictionary.Item(32, "Send RPG data to firebase app", "Job for sending RPG data to firebase app");
        public static DictionaryItem GetLastRPG => EntityDictionary.Item(33, "Get Last RPG data", "Job for getting Last RPG data");
        public static DictionaryItem GenerateStoryFromSummary => EntityDictionary.Item(34, "Generate story from summary", "Job for generating story from summary");
    }
}