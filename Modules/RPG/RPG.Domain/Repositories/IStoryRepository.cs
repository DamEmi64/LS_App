using Base;
using RPG.Domain.Entities;

namespace RPG.Domain.Repositories
{
    public interface IStoryRepository : IEntityRepository<Story>
    {
        Task<Story?> GetLastEdited();
        Task<string?> GetStoryTitle(Guid id);
        Task<Story?> GetFull(Guid id);
        Task<Story?> GetDraft(Guid id);
        IEnumerable<Story> GetAllDraft();
    }
}