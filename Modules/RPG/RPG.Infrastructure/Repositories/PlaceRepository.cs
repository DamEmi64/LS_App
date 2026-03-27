using Base;
using Microsoft.EntityFrameworkCore;
using RPG.Domain.Entities;
using RPG.Domain.Repositories;
using System.Infrastructure.Db;

namespace RPG.Infrastructure.Repositories
{
    public class PlaceRepository : EntityRepository<RPGContext, Place>, IPlaceRepository
    {
        public PlaceRepository(RPGContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Place?> Get(Guid id)
        {
            return await DbContext.Set<Place>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public IEnumerable<Place> GetByChapterId(Guid id)
        {
            return DbContext.Set<Place>().Include(x => x.Chapter).Where(x => x.Chapter.Id == id);
        }

        public IEnumerable<Place> GetWithStories()
        {
            return DbContext.Set<Place>().Include(x => x.Chapter);
        }
    }
}