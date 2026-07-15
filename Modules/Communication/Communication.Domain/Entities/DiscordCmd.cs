using Base;

namespace Communication.Domain.Entities
{
    public class DiscordCmd : Entity
    {
        public required string Cmd { get; set; }
        public string? Response { get; set; }
        public bool Active { get; set; }
    }
}
