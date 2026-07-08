using CommunicationBase.Dtos;

namespace CommunicationBase.Interfaces
{
    public interface IDiscordCommandDispatcher
    {
        Task<string?> DispatchAsync(
            string command,
            DiscordCommandContext context,
            CancellationToken cancellationToken = default);
    }
}
