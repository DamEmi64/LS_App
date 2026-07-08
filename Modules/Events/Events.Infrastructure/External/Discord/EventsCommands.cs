using CommunicationBase.Attributes;
using CommunicationBase.Dtos;
using CommunicationBase.Interfaces;
using Events.Domain.Repositories;

namespace Events.Infrastructure.External.Discord
{
    public class EventsCommands : IDiscordCommandsWrapper
    {
        private readonly IEventRepository _eventRepository;

        public EventsCommands(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [DiscordCommand("event", "Event details for {0}: {1}")]
        public async Task<string> GetEvent(DiscordCommandContext ctx)
        {
            var title = ctx.Arguments.Length > 0 ? ctx.Arguments[0] : null;

            if (title is null)
                return "Please provide an event title.";

            var eventEntity = await _eventRepository.GetByName(title);

            if (eventEntity is null)
                return "Event with this title not found.";

            return string.Format(ctx.Configuration ?? "Event details for {0}: {1}", eventEntity.Title, eventEntity.Description);
        }

        [DiscordCommand("closest-event", "Event details for {0}: {1}")]
        public async Task<string> GetClosestEvent(DiscordCommandContext ctx)
        {
            var closestEvent = await _eventRepository.GetClosestEvent();
            if (closestEvent is null)
                return "No upcoming events found.";

            return string.Format(ctx.Configuration ?? "Event details for {0}: {1}", closestEvent.Title, closestEvent.Description);
        }
    }
}
