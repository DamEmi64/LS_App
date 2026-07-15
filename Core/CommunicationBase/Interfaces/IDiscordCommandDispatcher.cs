using CommunicationBase.Dtos;

namespace CommunicationBase.Interfaces
{
    public interface IDiscordCommandDispatcher
    {
        Task<DiscordResponse?> DispatchAsync(
            string command,
            DiscordCommandContext context,
            CancellationToken cancellationToken = default);
    }
}
