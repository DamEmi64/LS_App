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
                                .Include(x => x.Chapters).FirstOrDefaultAsync(x => x.Id == id);
        }

        public override IEnumerable<Story> GetAll()
        {
            return DbContext.Set<Story>()
                    .Include(x => x.Chapters);
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
    }
}