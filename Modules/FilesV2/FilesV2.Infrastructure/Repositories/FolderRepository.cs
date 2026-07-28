using Base;
using FilesV2.Domain.Repositories;
using FilesV2.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace FilesV2.Infrastructure.Repositories
{
    public class FolderRepository : EntityRepository<FilesV2Context, Domain.Entities.Directory> , IFolderRepository
    {
        public FolderRepository(FilesV2Context dbContext) 
            : base(dbContext)
        {
        }

        public bool IsEmpty(Guid directoryId)
        {
            return DbContext.Set<Domain.Entities.Directory>()
                    .Any(x => x.Id == directoryId && x.Files.Count == 0 && x.Children.Count == 0);
        }
    }
}
