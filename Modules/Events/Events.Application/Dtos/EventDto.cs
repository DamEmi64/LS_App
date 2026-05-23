using Events.Domain.Entities;

namespace Events.Application.Dtos
{
    public class EventDto
    {
        public Guid Id { get; set;  }
        public string Title { get; set; } = "EVENT";
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public DateTime? EventDate { get; set; }
        public List<UserDto> Participates { get; set; } = new List<UserDto>();

    }
}
