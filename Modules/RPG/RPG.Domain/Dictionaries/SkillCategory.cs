using Base;

namespace RPG.Domain.Dictionaries
{
    [Dictionary("Skill category")]
    public class SkillCategory
    {
        public static DictionaryItem Strength => EntityDictionary.Item(10001, "Strength"); // Fixed typo
        public static DictionaryItem Dexterity => EntityDictionary.Item(10002, "Dexterity");
        public static DictionaryItem Intelligence => EntityDictionary.Item(10003, "Intelligence"); // Fixed typo
        public static DictionaryItem Communication => EntityDictionary.Item(10004, "Communication");
    }
}