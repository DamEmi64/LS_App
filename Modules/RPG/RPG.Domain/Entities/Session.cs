using Base;

namespace RPG.Domain.Entities
{
    public class Session : Entity
    {
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset? End { get; set; }
        public string? Summary { get; set; }
    }
}