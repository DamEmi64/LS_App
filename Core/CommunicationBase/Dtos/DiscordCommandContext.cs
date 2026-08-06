namespace CommunicationBase.Dtos
{
    public class DiscordCommandContext
    {
        public required string UserId { get; init; }

        public required string Username { get; init; }

        public required string Message { get; init; }

        public string[] Arguments { private get; init; } = [];

        public int NumberOfArguments => Arguments.Length;

        public string? Configuration { get; set; }

        public string? GetArgument(int index)
        {
            if (Arguments.Length > index)
                return Arguments[index];

            return null;
        }
    }
}