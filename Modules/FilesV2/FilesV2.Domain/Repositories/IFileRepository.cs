using Base;

namespace FilesV2.Domain.Repositories
{
    public interface IFileRepository : IEntityRepository<Entities.File>
    {
        Task<List<Entities.File>> GetFilesInDirectory(string directory);
        Task<List<Entities.File>> GetFilesByUser(string userId);
    }
}
