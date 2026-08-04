using Base;
using FilesV2.Domain.Repositories;
using FilesV2.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace FilesV2.Infrastructure.Repositories
{
    public class FolderRepository : EntityRepository<FilesV2Context, Domain.Entities.Directory>, IFolderRepository
    {
        public FolderRepository(FilesV2Context dbContext)
            : base(dbContext)
        {
        }

        public override Task<Domain.Entities.Directory?> Get(Guid id)
        {
            return DbContext.Set<Domain.Entities.Directory>()
                .Include(x => x.Children)
                .Include(x => x.Files)
                .Include(x => x.Parent)
                .FirstOrDefaultAsync(x => x.Id == id);

        }

        public bool IsEmpty(Guid directoryId)
        {
            return DbContext.Set<Domain.Entities.Directory>()
                    .Any(x => x.Id == directoryId && x.Files.Count == 0 && x.Children.Count == 0);
        }
    }
}
