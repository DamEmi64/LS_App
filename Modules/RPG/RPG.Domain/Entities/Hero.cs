using Base;
using System.Text.Json.Serialization;

namespace RPG.Domain.Entities
{
    public class Hero : Entity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Description { get; set; }
        public string? Player { get; set; }
        public Guid? Image { get; set; }

        [JsonIgnore]
        public required Chapter Chapter { get; set; }

        public PlayerData? PlayerData { get; set; }
    }
}