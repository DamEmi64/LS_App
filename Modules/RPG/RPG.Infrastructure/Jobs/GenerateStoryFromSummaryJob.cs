using Base;
using RPG.Domain.Entities;
using RPG.Domain.Repositories;
using RPG.Infrastructure.Models;

namespace RPG.Infrastructure.Jobs
{
    public class GenerateStoryFromSummaryJob : IJob
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<IJob> Children { get; set; } = new();

        public DateTimeOffset RequestDate => DateTimeOffset.Now;

        public string Name => $"Convert {Summary?.Title ?? string.Empty} to story model";

        public SummaryModel? Summary { get; set; }

        public int OperationId => Domain.Dictionaries.Operations.GenerateStoryFromSummary;

        public async Task Execute(IJobContext jobContext)
        {
            var chapterRepository = jobContext.Resolve<IChapterRepository>();
            var storyRepository = jobContext.Resolve<IStoryRepository>();
            var story = await ExecuteInternal(storyRepository, chapterRepository);
            jobContext.PassData(story.ToModel());
        }

        public async Task<Story> ExecuteInternal(IStoryRepository storyRepository, IChapterRepository chapterRepository)
        {
            ArgumentNullException.ThrowIfNull(Summary);
            var story = await storyRepository.Get(Summary.Id);

            var chapters = new List<Chapter>();

            if (Summary.All)
            {
                return new Story
                {
                    Id = Summary.Id,
                    Description = string.IsNullOrEmpty(Summary.Description) ? story?.Description ?? string.Empty : Summary.Description,
                    Title = Summary.Title,
                    Chapters = story?.Chapters ?? new List<Chapter>()
                };
            }

            foreach (var chapterId in Summary.Chapters)
            {
                var chapter = await chapterRepository.GetWithPlayerData(chapterId);

                if (chapter is not null)
                {
                    chapters.Add(chapter);
                }
            }

            return new Story
            {
                Id = Summary.Id,
                Description = string.IsNullOrEmpty(Summary.Description) ? story?.Description ?? string.Empty : Summary.Description,
                Title = Summary.Title,
                Chapters = chapters
            };
        }
    }
}