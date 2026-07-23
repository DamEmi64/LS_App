using Base;
using FilesV2.Domain.Enums;
using FilesV2.Domain.Repositories;
using FilesV2.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace FilesV2.Infrastructure.Repositories
{
    public class FileRepository : EntityRepository<FilesV2Context, Domain.Entities.File>, IFileRepository
    {
        public FileRepository(FilesV2Context dbContext) : base(dbContext)
        {
        }

        public override Task<Domain.Entities.File?> Get(Guid id)
        {
            return DbContext.Set<Domain.Entities.File>()
                .Include(x => x.Folder)
                .Include(x => x.Users)
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<Domain.Entities.File>> GetFilesByUser(string userId)
        {
            return DbContext.Set<Domain.Entities.File>()
                .Include(x => x.Folder)
                .Include(x => x.Users)
                .Include(x => x.Owner)
                .Where(x => x.Public || x.Owner.UserId.ToString() == userId || x.Users.Any(u => u.UserId.ToString() == userId))
                .ToListAsync();
        }

        public Task<List<Domain.Entities.File>> GetFilesInDirectory(string directory)
        {
            return DbContext.Set<Domain.Entities.File>()
                .Include(x => x.Folder)
                .Include(x => x.Users)
                .Include(x => x.Owner)
                .Where(x => (x.Folder != null && x.Folder.Title == directory))
                .ToListAsync();
        }

        public static bool HasReadAccess(Domain.Entities.File file, string userId) =>
    file.Public || file.Owner.UserId.ToString() == userId || file.Users.Any(u => u.UserId.ToString() == userId);

        public static bool HasWriteAccess(Domain.Entities.File file, string userId) =>
            file.Owner.UserId.ToString() == userId ||
            file.Users.Any(u => u.UserId.ToString() == userId && u.Privilage == Privilage.ReadWrite);
    }
}
