using Base;
using FilesV2.Domain.Entities;

namespace FilesV2.Domain.Repositories
{
    public interface IFolderRepository : IEntityRepository<Entities.Directory>
    {
        bool IsEmpty(Guid id);
    }
}
