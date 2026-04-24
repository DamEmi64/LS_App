using Base;

namespace RPG.Domain.Dictionaries
{
    [Dictionary("Skill category")]
    public class SkillCategory
    {
        public static DictionaryItem Strength => EntityDictionary.Item(401, "Strength"); // Fixed typo
        public static DictionaryItem Dexterity => EntityDictionary.Item(402, "Dexterity");
        public static DictionaryItem Intelligence => EntityDictionary.Item(403, "Intelligence"); // Fixed typo
        public static DictionaryItem Communication => EntityDictionary.Item(404, "Communication");
    }
}