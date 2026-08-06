using Base;
using RPG.Domain.Entities;
using RPG.Domain.Repositories;
using RPG.Infrastructure.Models;

namespace RPG.Infrastructure.Jobs.GenerateSummaryFromSummary
{
    public class GenerateStoryFromSummaryJobHandler : JobHandler<GenerateStoryFromSummaryJob>
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IChapterRepository _chapterRepository;

        public GenerateStoryFromSummaryJobHandler(IJobContext jobContext,
            IStoryRepository storyRepository,
            IChapterRepository chapterRepository)
            : base(jobContext)
        {
            _storyRepository = storyRepository;
            _chapterRepository = chapterRepository;
        }

        public override async Task Execute(GenerateStoryFromSummaryJob request)
        {
            ArgumentNullException.ThrowIfNull(request.Summary);
            var story = await _storyRepository.Get(request.Summary.Id);

            var chapters = new List<Chapter>();

            if (request.Summary.All)
            {
                var storyModel = new Story
                {
                    Id = request.Summary.Id,
                    Description = string.IsNullOrEmpty(request.Summary.Description) ? story?.Description ?? string.Empty : request.Summary.Description,
                    Title = request.Summary.Title,
                    Chapters = story?.Chapters ?? new List<Chapter>()
                }.ToModel();

                PassData(storyModel);
            }

            foreach (var chapterId in request.Summary.Chapters)
            {
                var chapter = await _chapterRepository.GetWithPlayerData(chapterId);

                if (chapter is not null)
                {
                    chapters.Add(chapter);
                }
            }

            PassData(new Story
            {
                Id = request.Summary.Id,
                Description = string.IsNullOrEmpty(request.Summary.Description) ? story?.Description ?? string.Empty : request.Summary.Description,
                Title = request.Summary.Title,
                Chapters = chapters
            }.ToModel());
        }
    }
}
