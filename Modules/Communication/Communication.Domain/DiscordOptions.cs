namespace Communication.Domain
{
    public class DiscordOptions
    {
        public required string Token { get; set; }
        public required string PublicKey { get; set; }
        public required string ApplicationId { get; set; }
    }
}
