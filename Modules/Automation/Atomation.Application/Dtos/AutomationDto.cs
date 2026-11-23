using Automation.Domain.Entities;

namespace Automation.Application.Dtos
{
    public class AutomationDto : Base.IDto<Automat, AutomationDto>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<TaskDto> Tasks { get; set; } = new();
        public List<TriggerDto> Triggers { get; set; } = new();
        public bool Active { get; set; }

        public Automat ToEntity() => new()
        {
            Id = Guid.NewGuid(), Title = Title, Description = Description, Active = Active,
            InsDate = DateTimeOffset.UtcNow, UpdDate = DateTimeOffset.UtcNow,
            Tasks = Tasks.Select(x => x.ToEntity()).ToList(),
            Triggers = Triggers.Select(x => x.ToEntity()).ToList()
        };

        public static AutomationDto ToDto(Automat entity) => new()
        {
            Id = entity.Id, Title = entity.Title, Description = entity.Description, Active = entity.Active,
            Tasks = entity.Tasks.Select(TaskDto.ToDto).ToList(),
            Triggers = entity.Triggers.Select(TriggerDto.ToDto).ToList()
        };

        public static explicit operator Automat(AutomationDto dto) => dto.ToEntity();
        public static explicit operator AutomationDto(Automat entity) => ToDto(entity);
    }
}
