using Events.Domain.Entities;
using Events.Domain.Enums;
namespace Events.Application.Dtos
{
    public class EventDto : Base.IDto<Event, EventDto>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "EVENT";
        public string? Description { get; set; }
        public int Category { get; set; }
        public DateTime? EventDate { get; set; }
        public List<UserDto> Participates { get; set; } = new();
        public Guid? Image { get; set; }
        public string? ImageContent { get; set; }
        public CircularPeriodEnum CircularPeriod { get; set; }

        public Event ToEntity()
        {
            var entity = new Event { Id = Id == Guid.Empty ? Guid.NewGuid() : Id, Title = Title, Description = Description, CategoryId = Category, EventDate = EventDate, Image = Image ?? Guid.Empty };
            entity.Participates = Participates.Select(p => p.ToEntity(entity)).ToList();
            entity.CircularPeriod = CircularPeriod;
            return entity;
        }
        public static EventDto ToDto(Event entity) => new()
        {
            Id = entity.Id, Title = entity.Title, Description = entity.Description, Category = entity.CategoryId,
            EventDate = entity.EventDate, Image = entity.Image, Participates = entity.Participates.Select(UserDto.ToDto).ToList(),
            CircularPeriod = entity.CircularPeriod
        };
        public static explicit operator Event(EventDto dto) => dto.ToEntity();
        public static explicit operator EventDto(Event entity) => ToDto(entity);
    }
}
