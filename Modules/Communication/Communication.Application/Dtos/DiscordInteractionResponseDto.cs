namespace Communication.Application.Dtos
{
    public class DiscordInteractionResponseDto
    {
        public int Type { get; set; }

        public DiscordInteractionDataDto? Data { get; set; }
    }
}
