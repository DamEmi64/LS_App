using Base;

namespace Files.Domain.Dictionaries
{
    [Dictionary("Game genres")]
    public static class GameGenres
    {
        public static DictionaryItem Platformer => EntityDictionary.Item(200, "Platformer");
        public static DictionaryItem Shooter => EntityDictionary.Item(201, "Shooter");
        public static DictionaryItem FirstPersonShooter => EntityDictionary.Item(202, "First Person Shooter");
        public static DictionaryItem ThirdPersonShooter => EntityDictionary.Item(203, "Third Person Shooter");
        public static DictionaryItem BeatEmUp => EntityDictionary.Item(204, "Beat 'Em Up");
        public static DictionaryItem Stealth => EntityDictionary.Item(205, "Stealth");
        public static DictionaryItem Survival => EntityDictionary.Item(206, "Survival");

        public static DictionaryItem ActionAdventure => EntityDictionary.Item(210, "Action Adventure");
        public static DictionaryItem NarrativeAdventure => EntityDictionary.Item(211, "Narrative Adventure");
        public static DictionaryItem PointAndClick => EntityDictionary.Item(212, "Point and Click");

        public static DictionaryItem ActionRPG => EntityDictionary.Item(220, "Action RPG");
        public static DictionaryItem TurnBasedRPG => EntityDictionary.Item(221, "Turn-Based RPG");
        public static DictionaryItem TacticalRPG => EntityDictionary.Item(222, "Tactical RPG");
        public static DictionaryItem MMORPG => EntityDictionary.Item(223, "MMORPG");
        public static DictionaryItem OpenWorldRPG => EntityDictionary.Item(224, "Open World RPG");

        public static DictionaryItem LifeSimulation => EntityDictionary.Item(230, "Life Simulation");
        public static DictionaryItem FarmingSimulation => EntityDictionary.Item(231, "Farming Simulation");
        public static DictionaryItem VehicleSimulation => EntityDictionary.Item(232, "Vehicle Simulation");
        public static DictionaryItem CityBuilding => EntityDictionary.Item(233, "City Building");

        public static DictionaryItem RealTimeStrategy => EntityDictionary.Item(240, "Real-Time Strategy");
        public static DictionaryItem TurnBasedStrategy => EntityDictionary.Item(241, "Turn-Based Strategy");
        public static DictionaryItem TowerDefense => EntityDictionary.Item(242, "Tower Defense");
        public static DictionaryItem FourX => EntityDictionary.Item(243, "4X Strategy");

        public static DictionaryItem ClassicPuzzle => EntityDictionary.Item(250, "Classic Puzzle");
        public static DictionaryItem PhysicsPuzzle => EntityDictionary.Item(251, "Physics Puzzle");
        public static DictionaryItem Match3 => EntityDictionary.Item(252, "Match 3");

        public static DictionaryItem TraditionalSports => EntityDictionary.Item(260, "Traditional Sports");
        public static DictionaryItem ExtremeSports => EntityDictionary.Item(261, "Extreme Sports");
        public static DictionaryItem Racing => EntityDictionary.Item(262, "Racing");

        public static DictionaryItem TraditionalFighting => EntityDictionary.Item(270, "Traditional Fighting");
        public static DictionaryItem PlatformFighting => EntityDictionary.Item(271, "Platform Fighting");

        public static DictionaryItem SurvivalHorror => EntityDictionary.Item(280, "Survival Horror");
        public static DictionaryItem PsychologicalHorror => EntityDictionary.Item(281, "Psychological Horror");

        public static DictionaryItem PartyGame => EntityDictionary.Item(290, "Party Game");
        public static DictionaryItem RhythmGame => EntityDictionary.Item(291, "Rhythm Game");

        public static DictionaryItem Other => EntityDictionary.Item(299, "Other");
    }
}