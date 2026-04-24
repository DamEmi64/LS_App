using Base;
using Microsoft.EntityFrameworkCore;
using RPG.Domain.Entities;
using RPG.Domain.Repositories;
using System.Infrastructure.Db;

namespace RPG.Infrastructure.Repositories
{
    public class PlaceRepository : EntityRepository<RPGContext, Place>, IPlaceRepository
    {
        private readonly IMediaProvider _mediaProvider;
        public PlaceRepository(RPGContext dbContext, IMediaProvider mediaProvider) : base(dbContext)
        {
            _mediaProvider = mediaProvider;
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

        public override async Task Remove(Guid id)
        {
            var place = await Get(id);

            if (place is null)
                return;

            await _mediaProvider.Delete(place.Image);
            DbContext.Set<Place>().Remove(place);
            DbContext.SaveChanges();
        }
    }
}