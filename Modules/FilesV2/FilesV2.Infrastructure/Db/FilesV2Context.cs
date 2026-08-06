using Base;
using Microsoft.EntityFrameworkCore;

namespace FilesV2.Infrastructure.Db
{
    public class FilesV2Context : DbContextBase<FilesV2Context>
    {
        public FilesV2Context(DbContextOptions<FilesV2Context> options, IEntityContext entityContext)
            : base(options, entityContext)
        {
        }

        public override string ContextName => "FilesV2";

        public DbSet<Domain.Entities.File> FilesV2 { get; set; } = default!;
        public DbSet<Domain.Entities.Directory> Directories { get; set; } = default!;
    }
}
