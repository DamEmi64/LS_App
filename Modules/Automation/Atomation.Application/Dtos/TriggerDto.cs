using Automation.Domain.Enums;

namespace Automation.Application.Dtos
{
    public class TriggerDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public TriggerType Type { get; set; }
        public string? Cron { get; set; }
        public List<int> EventId { get; set; } = new();
    }
}
