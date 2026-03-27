using Base;
using System.Text.Json.Serialization;

namespace RPG.Domain.Entities
{
    public class Place : Entity
    {
        public required string Title { get; set; }
        public required string Description { get; set; }

        [JsonIgnore]
        public required Chapter Chapter { get; set; }

        public Guid? Image { get; set; }
    }
}