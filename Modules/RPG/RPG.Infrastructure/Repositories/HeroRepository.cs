using Base;
using Microsoft.EntityFrameworkCore;
using RPG.Domain.Entities;
using RPG.Domain.Repositories;
using System.Infrastructure.Db;

namespace RPG.Infrastructure.Repositories
{
    public class HeroRepository : EntityRepository<RPGContext, Hero>, IHeroRepository
    {
        private readonly IMediaProvider _mediaProvider;
        public HeroRepository(RPGContext dbContext, IMediaProviderFactory mediaProviderFactory) : base(dbContext)
        {
            _mediaProvider = mediaProviderFactory.Create();
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

        public override async Task Remove(Guid id)
        {
            var hero = await DbContext.Set<Hero>().FirstOrDefaultAsync(x => x.Id == id);

            if (hero is null)
                return;

            await _mediaProvider.Delete(hero.Image);

            DbContext.Remove(hero);

            DbContext.SaveChanges();
        }
    }
}