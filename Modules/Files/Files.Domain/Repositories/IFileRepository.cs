using Base;

namespace Files.Domain.Repositories
{
    public interface IFileRepository : IEntityRepository<Entities.File>
    {
        Task ClearLinkCheck(Guid fileId);

        Task CheckLink(Guid linkId);
    }
}