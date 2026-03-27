using Base;

namespace RPG.Domain.Entities
{
    public class Skill : Entity
    {
        public required string Title { get; set; }
        public int CategoryId { get; set; }
        public decimal Value { get; set; }
    }
}