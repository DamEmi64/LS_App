using CommunicationBase.Attributes;
using CommunicationBase.Dtos;
using CommunicationBase.Interfaces;
using RPG.Domain.Repositories;


namespace RPG.Infrastructure.External.Discord
{
    public class RpgCommands : IDiscordCommandsWrapper
    {
        private readonly IStoryRepository _storyRepository;

        public RpgCommands(IStoryRepository storyRepository)
        {
            _storyRepository = storyRepository;
        }

        [DiscordCommand("last-rpg", "Last RPG session was {0}.")]
        public async Task<string> LastRpg(DiscordCommandContext ctx)
        {
            var story = await _storyRepository.GetLastEdited();

            if (story is null)
                return "No RPG story found.";

            return string.Format(ctx.Configuration ?? "Last RPG Session is {0}.", story.Title, story.Description);
        }
    }
}
