using System.Text.Json;

namespace CommunicationBase.Dtos
{
    public class DiscordCommandContext
    {
        public required string UserId { get; init; }

        public required string Username { get; init; }

        public required string Message { get; init; }

        public string[] Arguments { get; init; } = [];

        public string? Configuration { get; set; }
    }
}
