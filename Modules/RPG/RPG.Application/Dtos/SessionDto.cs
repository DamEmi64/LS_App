using Base.Helpers;
using RPG.Domain.Entities;

namespace RPG.Application.Dtos
{
    public class SessionDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset? End { get; set; }
    }
}