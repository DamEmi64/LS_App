using Base;
using RPG.Domain.Entities;

namespace RPG.Domain.Repositories
{
    public interface IStoryRepository : IEntityRepository<Story>
    {
        Task<Story?> GetLastEdited();
    }
}