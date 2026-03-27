using Automation.Domain.Enums;
using Base;

namespace Automation.Domain.Entities
{
    public class Trigger : Entity
    {
        public TriggerType Type { get; set; }
        public required string Cron { get; set; }   // store as string or milliseconds if needed
        public List<int> EventId { get; set; } = new();
    }
}
