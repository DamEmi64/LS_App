namespace Automation.Application.Dtos
{
    public class TriggerDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Cron { get; set; }
        public int EventId { get; set; }
    }
}
