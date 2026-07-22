using Base;
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

        [DiscordCommand("event", "Event details for {Title}: {Description}", "title:required")]
        public async Task<DiscordResponse> GetEvent(DiscordCommandContext ctx)
        {
            var title = ctx.GetArgument(0);

            if (title is null)
                return new DiscordResponse { Text = "Please provide an event title." };

            var eventEntity = await _eventRepository.GetByName(title);

            if (eventEntity is null)
                return new DiscordResponse { Text = "Event with this title not found." };

            return new DiscordResponse
            {
                Text = TemplateFormatter.Format(ctx.Configuration ?? "Event details for {Title}: {Description}", new { eventEntity.Title, eventEntity.Description, Participates = string.Join(",", eventEntity.Participates.Select(x => x.Login)) })
            };
        }

        [DiscordCommand("closest-event", "Event details for {Title}: {Description}")]
        public async Task<DiscordResponse> GetClosestEvent(DiscordCommandContext ctx)
        {
            var closestEvent = await _eventRepository.GetClosestEvent();
            if (closestEvent is null)
                return new DiscordResponse { Text = "No upcoming events found." };
            return new DiscordResponse
            {
                Text = TemplateFormatter.Format(ctx.Configuration ?? "Event details for {Title}: {Description}", new { closestEvent.Title, closestEvent.Description, Participates = string.Join(",", closestEvent.Participates.Select(x => x.Login)) })
            };
        }
    }
}
