using Base;

namespace FilesV2.Domain.Repositories
{
    public interface IFolderRepository : IEntityRepository<Entities.Directory>
    {
        bool IsEmpty(Guid id);
    }
}
