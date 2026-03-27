using Base;
using Microsoft.EntityFrameworkCore;
using RPG.Domain.Entities;
using RPG.Domain.Repositories;
using System.Infrastructure.Db;

namespace RPG.Infrastructure.Repositories
{
    public class ChapterRepository : EntityRepository<RPGContext, Chapter>, IChapterRepository
    {
        public ChapterRepository(RPGContext dbContext) : base(dbContext)
        {
        }

        public async Task<Chapter?> GetOnlyChapter(Guid id)
        {
            return await DbContext.Set<Chapter>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public override IEnumerable<Chapter> GetAll()
        {
            return DbContext.Set<Chapter>();
        }

        public override async Task<Chapter?> Get(Guid id)
        {
            return await DbContext.Set<Chapter>()
                            .Include(x => x.Sessions)
                            .Include(x => x.Links)
                            .Include(x => x.Heroes).AsNoTracking()
                            .Include(x => x.Places).AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Chapter?> GetWithPlayerData(Guid id)
        {
            return await DbContext.Set<Chapter>()
                            .Include(x => x.Sessions)
                            .Include(x => x.Links)
                            .Include(x => x.Heroes)
                            .ThenInclude(x => x.PlayerData).AsNoTracking()
                            .Include(x => x.Places).AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddLink(Link link)
        {
            await DbContext.Set<Link>().AddAsync(link);
            await DbContext.SaveChangesAsync();
        }

        public async Task RemoveLink(Link link)
        {
            DbContext.Set<Link>().Remove(link);
            await DbContext.SaveChangesAsync();
        }

        public async Task AddSession(Chapter chapter, Session session)
        {
            await DbContext.Set<Session>().AddAsync(session);
        }

        public async Task<Chapter?> GetWithStory(Guid id)
        {
            return await DbContext.Set<Chapter>()
                            .Include(x => x.Story)
                            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Chapter?> GetWithStoryAndSessions(Guid id)
        {
            return await DbContext.Set<Chapter>()
                            .Include(x => x.Story)
                            .Include(x => x.Sessions)
                            .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}