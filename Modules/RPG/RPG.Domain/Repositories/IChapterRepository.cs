using Base;
using RPG.Domain.Entities;

namespace RPG.Domain.Repositories
{
    public interface IChapterRepository : IEntityRepository<Chapter>
    {
        Task<Chapter?> GetOnlyChapter(Guid id);

        Task<Chapter?> GetWithStory(Guid id);

        Task<Chapter?> GetWithStoryAndSessions(Guid id);

        Task AddLink(Link link);

        Task RemoveLink(Link link);

        Task AddSession(Chapter chapter, Session session);

        Task<Chapter?> GetWithPlayerData(Guid id);
    }
}