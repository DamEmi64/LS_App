using Automation.Domain.Entities;
namespace Automation.Application.Dtos
{
    public class TriggerDto : Base.IDto<Trigger, TriggerDto>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Cron { get; set; }
        public int EventId { get; set; }
        public Trigger ToEntity() => new() { Id = Guid.NewGuid(), Cron = Cron ?? string.Empty, EventId = EventId, InsDate = DateTimeOffset.UtcNow, UpdDate = DateTimeOffset.UtcNow };
        public static TriggerDto ToDto(Trigger entity) => new() { Id = entity.Id, Cron = entity.Cron, EventId = entity.EventId };
        public static explicit operator Trigger(TriggerDto dto) => dto.ToEntity();
        public static explicit operator TriggerDto(Trigger entity) => ToDto(entity);
    }
}
