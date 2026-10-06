using Base;

namespace Files.Domain.Dictionaries
{
    [Dictionary("Game genres")]
    public static class GameGenres
    {
        public static DictionaryItem Platformer => EntityDictionary.Item(50201, "Platformer");
        public static DictionaryItem Shooter => EntityDictionary.Item(50202, "Shooter");
        public static DictionaryItem FirstPersonShooter => EntityDictionary.Item(50203, "First Person Shooter");
        public static DictionaryItem ThirdPersonShooter => EntityDictionary.Item(50204, "Third Person Shooter");
        public static DictionaryItem BeatEmUp => EntityDictionary.Item(50205, "Beat 'Em Up");
        public static DictionaryItem Stealth => EntityDictionary.Item(50206, "Stealth");
        public static DictionaryItem Survival => EntityDictionary.Item(50207, "Survival");

        public static DictionaryItem ActionAdventure => EntityDictionary.Item(50208, "Action Adventure");
        public static DictionaryItem NarrativeAdventure => EntityDictionary.Item(50209, "Narrative Adventure");
        public static DictionaryItem PointAndClick => EntityDictionary.Item(50210, "Point and Click");

        public static DictionaryItem ActionRPG => EntityDictionary.Item(50211, "Action RPG");
        public static DictionaryItem TurnBasedRPG => EntityDictionary.Item(50212, "Turn-Based RPG");
        public static DictionaryItem TacticalRPG => EntityDictionary.Item(50213, "Tactical RPG");
        public static DictionaryItem MMORPG => EntityDictionary.Item(50214, "MMORPG");
        public static DictionaryItem OpenWorldRPG => EntityDictionary.Item(50215, "Open World RPG");

        public static DictionaryItem LifeSimulation => EntityDictionary.Item(50216, "Life Simulation");
        public static DictionaryItem FarmingSimulation => EntityDictionary.Item(50217, "Farming Simulation");
        public static DictionaryItem VehicleSimulation => EntityDictionary.Item(50218, "Vehicle Simulation");
        public static DictionaryItem CityBuilding => EntityDictionary.Item(50219, "City Building");

        public static DictionaryItem RealTimeStrategy => EntityDictionary.Item(50220, "Real-Time Strategy");
        public static DictionaryItem TurnBasedStrategy => EntityDictionary.Item(50221, "Turn-Based Strategy");
        public static DictionaryItem TowerDefense => EntityDictionary.Item(50222, "Tower Defense");
        public static DictionaryItem FourX => EntityDictionary.Item(50223, "4X Strategy");

        public static DictionaryItem ClassicPuzzle => EntityDictionary.Item(50224, "Classic Puzzle");
        public static DictionaryItem PhysicsPuzzle => EntityDictionary.Item(50225, "Physics Puzzle");
        public static DictionaryItem Match3 => EntityDictionary.Item(50226, "Match 3");

        public static DictionaryItem TraditionalSports => EntityDictionary.Item(50227, "Traditional Sports");
        public static DictionaryItem ExtremeSports => EntityDictionary.Item(50228, "Extreme Sports");
        public static DictionaryItem Racing => EntityDictionary.Item(50229, "Racing");

        public static DictionaryItem TraditionalFighting => EntityDictionary.Item(50230, "Traditional Fighting");
        public static DictionaryItem PlatformFighting => EntityDictionary.Item(50231, "Platform Fighting");

        public static DictionaryItem SurvivalHorror => EntityDictionary.Item(50232, "Survival Horror");
        public static DictionaryItem PsychologicalHorror => EntityDictionary.Item(50233, "Psychological Horror");

        public static DictionaryItem PartyGame => EntityDictionary.Item(50234, "Party Game");
        public static DictionaryItem RhythmGame => EntityDictionary.Item(50235, "Rhythm Game");

        public static DictionaryItem Other => EntityDictionary.Item(50236, "Other");
    }
}