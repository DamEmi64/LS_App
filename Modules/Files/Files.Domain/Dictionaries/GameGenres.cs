using Base;

namespace Files.Domain.Dictionaries
{
    [Dictionary("Game genres")]
    public static class GameGenres
    {
        public static DictionaryItem Platformer => EntityDictionary.Item(4001, "Platformer");
        public static DictionaryItem Shooter => EntityDictionary.Item(4002, "Shooter");
        public static DictionaryItem FirstPersonShooter => EntityDictionary.Item(4003, "First Person Shooter");
        public static DictionaryItem ThirdPersonShooter => EntityDictionary.Item(4004, "Third Person Shooter");
        public static DictionaryItem BeatEmUp => EntityDictionary.Item(4005, "Beat 'Em Up");
        public static DictionaryItem Stealth => EntityDictionary.Item(4006, "Stealth");
        public static DictionaryItem Survival => EntityDictionary.Item(4007, "Survival");

        public static DictionaryItem ActionAdventure => EntityDictionary.Item(4008, "Action Adventure");
        public static DictionaryItem NarrativeAdventure => EntityDictionary.Item(4009, "Narrative Adventure");
        public static DictionaryItem PointAndClick => EntityDictionary.Item(4010, "Point and Click");

        public static DictionaryItem ActionRPG => EntityDictionary.Item(4011, "Action RPG");
        public static DictionaryItem TurnBasedRPG => EntityDictionary.Item(4012, "Turn-Based RPG");
        public static DictionaryItem TacticalRPG => EntityDictionary.Item(4013, "Tactical RPG");
        public static DictionaryItem MMORPG => EntityDictionary.Item(4014, "MMORPG");
        public static DictionaryItem OpenWorldRPG => EntityDictionary.Item(4015, "Open World RPG");

        public static DictionaryItem LifeSimulation => EntityDictionary.Item(4016, "Life Simulation");
        public static DictionaryItem FarmingSimulation => EntityDictionary.Item(4017, "Farming Simulation");
        public static DictionaryItem VehicleSimulation => EntityDictionary.Item(4018, "Vehicle Simulation");
        public static DictionaryItem CityBuilding => EntityDictionary.Item(4019, "City Building");

        public static DictionaryItem RealTimeStrategy => EntityDictionary.Item(4020, "Real-Time Strategy");
        public static DictionaryItem TurnBasedStrategy => EntityDictionary.Item(4021, "Turn-Based Strategy");
        public static DictionaryItem TowerDefense => EntityDictionary.Item(4022, "Tower Defense");
        public static DictionaryItem FourX => EntityDictionary.Item(4023, "4X Strategy");

        public static DictionaryItem ClassicPuzzle => EntityDictionary.Item(4024, "Classic Puzzle");
        public static DictionaryItem PhysicsPuzzle => EntityDictionary.Item(4025, "Physics Puzzle");
        public static DictionaryItem Match3 => EntityDictionary.Item(4026, "Match 3");

        public static DictionaryItem TraditionalSports => EntityDictionary.Item(4027, "Traditional Sports");
        public static DictionaryItem ExtremeSports => EntityDictionary.Item(4028, "Extreme Sports");
        public static DictionaryItem Racing => EntityDictionary.Item(4029, "Racing");

        public static DictionaryItem TraditionalFighting => EntityDictionary.Item(4030, "Traditional Fighting");
        public static DictionaryItem PlatformFighting => EntityDictionary.Item(4031, "Platform Fighting");

        public static DictionaryItem SurvivalHorror => EntityDictionary.Item(4032, "Survival Horror");
        public static DictionaryItem PsychologicalHorror => EntityDictionary.Item(4033, "Psychological Horror");

        public static DictionaryItem PartyGame => EntityDictionary.Item(4034, "Party Game");
        public static DictionaryItem RhythmGame => EntityDictionary.Item(4035, "Rhythm Game");

        public static DictionaryItem Other => EntityDictionary.Item(4036, "Other");
    }
}