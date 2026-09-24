using Base;
using FilesV2.Domain.Enums;
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
                .Include(x => x.Users)
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == id);

        }

        public override IEnumerable<Domain.Entities.Directory> GetAll()
        {
            return DbContext.Set<Domain.Entities.Directory>()
              .Include(x => x.Children)
              .Include(x => x.Owner);
        }

        public Task<List<Domain.Entities.Directory>> GetDirectoriesByUser(string userId)
        {
            return DbContext.Set<Domain.Entities.Directory>()
                .Include(x => x.Children)
                .Include(x => x.Files)
                .Include(x => x.Parent)
                .Include(x => x.Users)
                .Include(x => x.Owner)
                .Where(x => x.Public || x.Owner.UserId.ToString() == userId || x.Users.Any(u => u.UserId == userId))
                .ToListAsync();
        }

        public bool IsEmpty(Guid directoryId)
        {
            return DbContext.Set<Domain.Entities.Directory>()
                    .Any(x => x.Id == directoryId && x.Files.Count == 0 && x.Children.Count == 0);
        }

        public static bool HasReadAccess(Domain.Entities.Directory directory, string userId) =>
directory.Public || directory.Owner.UserId.ToString() == userId || directory.Users.Any(u => u.UserId.ToString() == userId);

        public static bool HasWriteAccess(Domain.Entities.Directory directory, string userId) =>
            directory.Owner.UserId.ToString() == userId ||
            directory.Users.Any(u => u.UserId.ToString() == userId && u.Privilage == Privilage.Write);
    }
}
