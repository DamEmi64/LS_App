using Base;

namespace RPG.Domain.Dictionaries
{
    [Dictionary("System automats")]
    public class SystemAutomats
    {
        public static DictionaryItem GenerateSummary => EntityDictionary.Item(140401, "Generate rpg summary");
    }
}
