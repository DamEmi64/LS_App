using Base;

namespace Automation.Domain.Entities
{
    public class Trigger : Entity
    {
        public int EventId { get; set; }
        public string? Cron { get; set; }   // store as string or milliseconds if needed
    }
}
