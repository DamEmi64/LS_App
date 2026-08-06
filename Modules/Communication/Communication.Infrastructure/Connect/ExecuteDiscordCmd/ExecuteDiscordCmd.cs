using CommunicationBase.Dtos;
using CommunicationBase.Events;
using CommunicationBase.Interfaces;

namespace Communication.Infrastructure.Connect.ExecuteDiscordCmd
{
    public class ExecuteDiscordCmd : Base.EventHandler<ExecuteDiscordCmdEvent, DiscordResponse>
    {
        private readonly IDiscordCommandDispatcher _discordCommandDispatcher;

        public ExecuteDiscordCmd(IDiscordCommandDispatcher discordCommandDispatcher)
        {
            _discordCommandDispatcher = discordCommandDispatcher;
        }

        public override async Task<DiscordResponse> HandleAsync(ExecuteDiscordCmdEvent request, CancellationToken cancellationToken)
        {
            return await _discordCommandDispatcher.DispatchAsync(request.Command, request.Context) ?? new DiscordResponse();
        }
    }
}
