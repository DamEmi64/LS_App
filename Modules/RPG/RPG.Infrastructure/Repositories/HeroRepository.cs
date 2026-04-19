using Base;
using Microsoft.EntityFrameworkCore;
using RPG.Domain.Entities;
using RPG.Domain.Repositories;
using System.Infrastructure.Db;

namespace RPG.Infrastructure.Repositories
{
    public class HeroRepository : EntityRepository<RPGContext, Hero>, IHeroRepository
    {
        public HeroRepository(RPGContext dbContext) : base(dbContext)
        {
        }

        public override IEnumerable<Hero> GetAll()
        {
            return DbContext.Set<Hero>().Include(x => x.PlayerData).ThenInclude(x => x!.Skills);
        }

        public override async Task<Hero?> Get(Guid id)
        {
            return await DbContext.Set<Hero>()
                .Include(x => x.Chapter)
                .Include(x => x.PlayerData)
                .ThenInclude(x => x!.Skills).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}