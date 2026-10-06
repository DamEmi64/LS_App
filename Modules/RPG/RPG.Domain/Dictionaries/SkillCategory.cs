using Base;

namespace RPG.Domain.Dictionaries
{
    [Dictionary("Skill category")]
    public class SkillCategory
    {
        public static DictionaryItem Strength => EntityDictionary.Item(110401, "Strength"); // Fixed typo
        public static DictionaryItem Dexterity => EntityDictionary.Item(110402, "Dexterity");
        public static DictionaryItem Intelligence => EntityDictionary.Item(110403, "Intelligence"); // Fixed typo
        public static DictionaryItem Communication => EntityDictionary.Item(110404, "Communication");
    }
}