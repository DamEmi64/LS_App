using Base;
using FilesV2.Domain.Repositories;
using FilesV2.Infrastructure.Db;

namespace FilesV2.Infrastructure.Repositories
{
    public class FolderRepository : EntityRepository<FilesV2Context, Domain.Entities.Directory> , IFolderRepository
    {
        public FolderRepository(FilesV2Context dbContext) 
            : base(dbContext)
        {
        }
    }
}
