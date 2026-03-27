using Base;
using RPG.Domain.Entities;

namespace RPG.Domain.Repositories
{
    public interface IPlaceRepository : IEntityRepository<Place>
    {
        IEnumerable<Place> GetByChapterId(Guid id);

        IEnumerable<Place> GetWithStories();
    }
}