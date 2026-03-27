using Base;

namespace RPG.Domain.Entities
{
    public class PlayerData : Entity
    {
        public string? Content { get; set; }

        public List<Skill> Skills { get; set; } = new List<Skill>();
    }
}