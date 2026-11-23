using Newtonsoft.Json;
using Automation.Domain.Entities;

namespace Automation.Application.Dtos
{
    public class TaskDto : Base.IDto<Automation.Domain.Entities.Task, TaskDto>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int OperationId { get; set; }
        public int Order { get; set; }
        public object? Data { get; set; }
        public Automation.Domain.Entities.Task ToEntity() => new()
        {
            Id = Guid.NewGuid(), OperationId = OperationId, Order = Order,
            Data = JsonConvert.SerializeObject(Data), InsDate = DateTimeOffset.UtcNow, UpdDate = DateTimeOffset.UtcNow
        };
        public static TaskDto ToDto(Automation.Domain.Entities.Task entity) => new()
        {
            Id = entity.Id, OperationId = entity.OperationId, Order = entity.Order,
            Data = JsonConvert.DeserializeObject(entity.Data)
        };
        public static explicit operator Automation.Domain.Entities.Task(TaskDto dto) => dto.ToEntity();
        public static explicit operator TaskDto(Automation.Domain.Entities.Task entity) => ToDto(entity);
    }
}
