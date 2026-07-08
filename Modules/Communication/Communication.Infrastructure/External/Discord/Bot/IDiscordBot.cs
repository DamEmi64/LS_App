namespace Communication.Infrastructure.External.Discord
{
    public interface IDiscordBot
    {
        Task StartAsync(CancellationToken cancellationToken = default);
    }
}
