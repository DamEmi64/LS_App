using Base;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace System.Infrastructure.Db
{
    public class FilesContext : DbContextBase<FilesContext>, IDbContextBase
    {
        public FilesContext(DbContextOptions<FilesContext> options, IEntityContext entityContext)
        : base(options, entityContext)
        {
        }

        public override string ContextName => "File System";

        public DbSet<Files.Domain.Entities.File> Files { get; set; } = default!;
        public DbSet<Files.Domain.Entities.SourceLink> Sources { get; set; } = default!;
    }
}