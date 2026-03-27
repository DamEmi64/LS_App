using Base;

namespace Automation.Domain.Entities
{
    public class Automat : Entity
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public List<Task> Tasks { get; set; } = new();
        public List<Trigger> Triggers { get; set; } = new();
        public DateTimeOffset? LastRun { get; set; }
        public bool Active { get; set; }
    }

}