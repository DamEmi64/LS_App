using Base;
using Files.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Infrastructure.Db;

namespace Files.Infrastructure.Repositories
{
    public class FileRepository : EntityRepository<FilesContext, Domain.Entities.File>, IFileRepository
    {
        private readonly IMediaProvider _mediaProvider;
        public FileRepository(FilesContext dbContext, IMediaProvider mediaProvider) : base(dbContext)
        {
            _mediaProvider = mediaProvider;
        }

        public async Task ClearLinkCheck(Guid fileId)
        {
            var file = await Get(fileId);

            if (file is null)
            {
                return;
            }

            foreach (var link in file.Sources)
            {
                link.Imported = false;
            }

            DbContext.Set<Domain.Entities.File>().Update(file);
            await DbContext.SaveChangesAsync();
        }

        public override async Task Remove(Guid id)
        {
            var file = DbContext.Set<Domain.Entities.File>()
                        .FirstOrDefault(x => x.Id == id);
            if (file is null)
                return;

            await _mediaProvider.Delete(file.Image);
            await _mediaProvider.Delete(file.Content);

            DbContext.Remove(file);
            DbContext.SaveChanges();
        }

        public async Task CheckLink(Guid linkId)
        {
            var link = await DbContext.Set<Domain.Entities.SourceLink>().FirstOrDefaultAsync(x => x.Id == linkId);
            if (link is null)
            {
                return;
            }

            link.Imported = true;

            DbContext.Set<Domain.Entities.SourceLink>().Update(link);
            await DbContext.SaveChangesAsync();
        }

        public override Task<Domain.Entities.File?> Get(Guid id)
        {
            return DbContext.Set<Domain.Entities.File>()
                .Include(x => x.AdditionalData)
                .Include(x => x.Sources)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}