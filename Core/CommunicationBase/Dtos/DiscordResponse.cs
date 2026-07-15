namespace CommunicationBase.Dtos
{
    public class DiscordResponse
    {
        public string Text { get; set; } = string.Empty;
        public byte[] File { get; set; } = Array.Empty<byte>();
    }
}
