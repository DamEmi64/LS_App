using Base;
using CommunicationBase.Dtos;
using CommunicationBase.Events;
using CommunicationBase.Interfaces;

namespace Communication.Infrastructure.Connect.ExecuteDiscordCmd
{
    public class ExecuteDiscordCmd : ConnectInstance<ExecuteDiscordCmdEvent,DiscordResponse>
    {
        private readonly IDiscordCommandDispatcher _discordCommandDispatcher;

        public ExecuteDiscordCmd(IDiscordCommandDispatcher discordCommandDispatcher)
        {
            _discordCommandDispatcher = discordCommandDispatcher;
        }

        public override async Task<DiscordResponse> HandleAsync(ExecuteDiscordCmdEvent request)
        {
            return await _discordCommandDispatcher.DispatchAsync(request.Command, request.Context) ?? new DiscordResponse();
        }
    }
}
