using Events.Domain.Entities;
namespace Events.Application.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public string? Login { get; set; }
        public string? Email { get; set; }
        public bool Present { get; set; }
        public EventUser ToEntity(Event parent) => new() { Id = Id, UserId = UserId ?? string.Empty, Login = Login, Email = Email, Present = Present, Event = parent };
        public static UserDto ToDto(EventUser entity) => new() { Id = entity.Id, UserId = entity.UserId, Login = entity.Login, Email = entity.Email, Present = entity.Present };
    }
}
