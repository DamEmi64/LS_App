using Base;
using Events.Domain.Enums;

namespace Events.Domain.Entities
{
    public class Event : Entity
    {
        public required string Title { get; set; }

        public string? Description { get; set; }

        public int CategoryId { get; set; }

        public Guid Image { get; set; }

        public DateTime? EventDate { get; set; }

        public List<EventUser> Participates { get; set; } = new List<EventUser>();

        public Guid? ReminderProcess { get; set; }
        public CircularPeriodEnum CircularPeriod { get; set; } = CircularPeriodEnum.None;
        public DateTime? CircularStartDate { get; set; }
    }
}