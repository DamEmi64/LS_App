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
                                .Include(x => x.Chapters.Where(x => !x.Draft)).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Story?> GetDraft(Guid id)
        {
            return await DbContext.Set<Story>()
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
            return await DbContext.Stories
                .Include(x => x.Chapters)
                .ThenInclude(x => x.Heroes)
                .Include(x => x.Chapters)
                .ThenInclude(x => x.Places)
                .Select(story => new
                {
                    Story = story,
                    LastEdit = new[]
                    {
                        story.UpdDate,
                        story.Chapters.Select(c => c.UpdDate).DefaultIfEmpty(DateTime.MinValue).Max(),
                        story.Chapters.SelectMany(c => c.Heroes).Select(h => h.UpdDate).DefaultIfEmpty(DateTime.MinValue).Max(),
                        story.Chapters.SelectMany(c => c.Places).Select(p => p.UpdDate).DefaultIfEmpty(DateTime.MinValue).Max()
                    }.Max()
                })
                .OrderByDescending(x => x.LastEdit)
                .Select(x => x.Story)
                .FirstOrDefaultAsync();
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
    }
}