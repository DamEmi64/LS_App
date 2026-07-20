using Base;
using Microsoft.EntityFrameworkCore;
using RPG.Domain.Entities;
using RPG.Domain.Repositories;
using System.Infrastructure.Db;

namespace RPG.Infrastructure.Repositories
{
    public class StoryRepository : EntityRepository<RPGContext, Story>, IStoryRepository
    {
        public StoryRepository(RPGContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Story?> Get(Guid id)
        {
            return await DbContext.Set<Story>()
                                .Include(x => x.Files)
                                .Include(x => x.Chapters.Where(x => !x.Draft)).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Story?> GetDraft(Guid id)
        {
            return await DbContext.Set<Story>()
                                .Include(x => x.Files)
                                .Include(x => x.Chapters.Where(x => x.Draft)).FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Story?> GetFull(Guid id)
        => DbContext.Stories
            .Include(x => x.Chapters)
            .ThenInclude(x => x.Heroes)
            .ThenInclude(x => x.PlayerData)
            .Include(x => x.Chapters)
            .ThenInclude(x => x.Places)
            .Include(x => x.Chapters)
            .ThenInclude(x => x.Links)
            .FirstOrDefaultAsync(x => x.Id == id);

        public override IEnumerable<Story> GetAll()
        {
            return DbContext.Set<Story>()
                    .Include(x => x.Chapters.Where(x => !x.Draft));
        }

        public IEnumerable<Story> GetAllDraft()
        {
            return DbContext.Set<Story>()
                    .Include(x => x.Chapters.Where(x => x.Draft));
        }

        public async Task<Story?> GetLastEdited()
        {
            var latestStory = await DbContext.Stories
                .Select(x => new
                {
                    Story = x,
                    x.UpdDate
                })
                .OrderByDescending(x => x.UpdDate)
                .FirstOrDefaultAsync();

            var latestChapter = await DbContext.Chapters
                .Include(x => x.Story)
                .Select(x => new
                {
                    x.Story,
                    x.UpdDate
                })
                .OrderByDescending(x => x.UpdDate)
                .FirstOrDefaultAsync();

            var latestHero = await DbContext.Heroes
                .Include(x=>x.Chapter)
                .ThenInclude(x=>x.Story)
                .Select(x => new
                {
                    x.Chapter.Story,
                    x.UpdDate
                })
                .OrderByDescending(x => x.UpdDate)
                .FirstOrDefaultAsync();

            var latestPlace = await DbContext.Places
                .Include(x => x.Chapter)
                .ThenInclude(x => x.Story)
                .Select(x => new
                {
                    x.Chapter.Story,
                    x.UpdDate
                })
                .OrderByDescending(x => x.UpdDate)
                .FirstOrDefaultAsync();

            var candidates = new[]
            {
                latestStory,
                latestChapter,
                latestHero,
                latestPlace
            }
            .Where(x => x != null)
            .OrderByDescending(x => x!.UpdDate)
            .First();

            return await DbContext.Stories
                .Include(x => x.Chapters)
                    .ThenInclude(x => x.Heroes)
                .Include(x => x.Chapters)
                    .ThenInclude(x => x.Places)
                .FirstOrDefaultAsync(x => x.Id == candidates!.Story.Id);
        }

        public Task<string?> GetStoryTitle(Guid id)
        => DbContext.Stories.Where(x => x.Id == id).Select(x => x.Title).FirstOrDefaultAsync();

        public override async Task Remove(Guid id)
        {
            var entity = await GetFull(id);
            if (entity is not null)
            {
                DbContext.Stories.Remove(entity);
                await DbContext.SaveChangesAsync();
            }
        }

        public async Task AddFile(Story story, RPGFile file)
        {
            await DbContext.Set<RPGFile>().AddAsync(file);
            story.Files.Add(file);
            DbContext.Update(story);
            await DbContext.SaveChangesAsync();
        }
    }
}