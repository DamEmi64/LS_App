using Base.Connect;
using CommunicationBase.Dtos;

namespace CommunicationBase.Events
{
    public record ExecuteDiscordCmdEvent(
        string Command,
        DiscordCommandContext Context
    ) : IEvent<DiscordResponse>;
}
