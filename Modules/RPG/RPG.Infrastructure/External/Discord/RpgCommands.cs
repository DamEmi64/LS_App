using Base;
using CommunicationBase.Attributes;
using CommunicationBase.Dtos;
using CommunicationBase.Interfaces;
using RPG.Domain.Repositories;
using RPG.Infrastructure.Models;
using RPG.Infrastructure.Services.SummaryService;


namespace RPG.Infrastructure.External.Discord
{
    public class RpgCommands : IDiscordCommandsWrapper
    {
        private readonly IStoryRepository _storyRepository;
        private readonly ISummaryService _summaryService;
        private readonly IMediaProvider _mediaProvider;

        public RpgCommands(IStoryRepository storyRepository, ISummaryService summaryService, IMediaProvider mediaProvider)
        {
            _storyRepository = storyRepository;
            _summaryService = summaryService;
            _mediaProvider = mediaProvider;
        }

        [DiscordCommand("last-rpg", "Last RPG session was {Title}.")]
        public async Task<DiscordResponse> LastRpg(DiscordCommandContext ctx)
        {
            var story = await _storyRepository.GetLastEdited();

            if (story is null)
                return new DiscordResponse { Text = "No RPG story found." };

            return new DiscordResponse
            {
                Text = string.Format(ctx.Configuration ?? "Last RPG Session is {Title}.", new { story.Title, story.Description })
            };
        }

        [DiscordCommand("summary", "Last RPG session was {Title}.")]
        public async Task<DiscordResponse> Summary(DiscordCommandContext ctx)
        {
            var story = await _storyRepository.GetLastEdited();

            if (story is null)
                return new DiscordResponse { Text = "No RPG story found." };

            if (story.Summary is null)
            {
                var dto = new SummaryModel
                {
                    Chapters = story.Chapters.Select(c => c.Id).ToList(),
                    Description = story.Description,
                    Title = story.Title,
                    IsPdf = ctx.Arguments[0] != "html"
                };
                var job = await _summaryService.QueueGenerateSummaryJob(story.Id, dto, UserData.System, dto.IsPdf);
                return new DiscordResponse { Text = "No RPG summary found. Requesting for summary generation, wait few minutes and ask again..." };
            }

            var media = await _mediaProvider.Load(story.Summary ?? Guid.Empty);

            return new DiscordResponse
            {
                Text = string.Format(ctx.Configuration ?? "Last RPG Session is {Title}.", new { story.Title, story.Description }),
                File = media?.Content ?? Array.Empty<byte>()
            };
        }
    }
}
