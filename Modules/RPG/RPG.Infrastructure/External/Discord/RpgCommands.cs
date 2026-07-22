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

        public RpgCommands(IStoryRepository storyRepository, ISummaryService summaryService, IMediaProviderFactory mediaProviderFactory)
        {
            _storyRepository = storyRepository;
            _summaryService = summaryService;
            _mediaProvider = mediaProviderFactory.Create(AppConfiguration.GetValue<string>("DefaultStorage"));
        }

        [DiscordCommand("last-rpg", "Last RPG session was {Title}.")]
        public async Task<DiscordResponse> LastRpg(DiscordCommandContext ctx)
        {
            var story = await _storyRepository.GetLastEdited();

            if (story is null)
                return new DiscordResponse { Text = "No RPG story found." };

            return new DiscordResponse
            {
                Text = TemplateFormatter.Format(ctx.Configuration ?? "Last RPG Session is {Title}.", new { story.Title, story.Description })
            };
        }

        [DiscordCommand("summary", "Last RPG session was {Title}.")]
        public async Task<DiscordResponse> Summary(DiscordCommandContext ctx)
        {
            var story = await _storyRepository.GetLastEdited();

            if (story is null)
                return new DiscordResponse { Text = "No RPG story found." };

            var isPdf = ctx.GetArgument(0) != "html";

            if (story.Summary is null)
            {
                var dto = new SummaryModel
                {
                    Chapters = story.Chapters.Select(c => c.Id).ToList(),
                    Description = story.Description,
                    Title = story.Title,
                    IsPdf = isPdf
                };
                var job = await _summaryService.QueueGenerateSummaryJob(story.Id, dto, UserData.System, dto.IsPdf);
                return new DiscordResponse { Text = "No RPG summary found. Requesting for summary generation, wait few minutes and ask again..." };
            }

            var media = await _mediaProvider.Load(story.Summary ?? Guid.Empty);

            return new DiscordResponse
            {
                Text = TemplateFormatter.Format(ctx.Configuration ?? "Last RPG Session is {Title}.", new { story.Title, story.Description }),
                Files = media?.Content is null ? null : new() { new()
                {
                    Content = media.Content,
                    Title = story.Title,
                    Extension = isPdf ? "pdf" : "html"
                } }
            };
        }
    }
}
