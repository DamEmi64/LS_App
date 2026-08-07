using Newtonsoft.Json;

namespace Events.Domain.Entities
{
    public class EventUser
    {
        public Guid Id { get; set; }

        public required string UserId { get; set; }

        public string? Login { get; set; }

        public string? Email { get; set; }

        public bool Invited { get; set; }
        public bool Present { get; set; }

        [JsonIgnore]
        public required Event Event { get; set; }
    }
}
