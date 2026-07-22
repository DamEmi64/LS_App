namespace Communication.Application.Dtos
{
    public class DiscordApplicationCommandDataDto
    {
        public string Name { get; set; } = string.Empty;

        public Dictionary<string, object>? Options { get; set; }
    }
}
