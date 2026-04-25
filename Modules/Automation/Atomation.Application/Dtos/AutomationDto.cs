namespace Automation.Application.Dtos
{
    public class AutomationDto
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<TaskDto> Tasks { get; set; } = new();
        public List<TriggerDto> Triggers { get; set; } = new();
        public bool Active { get; set; }
    }
}
