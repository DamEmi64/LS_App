using Base;
using Microsoft.EntityFrameworkCore;
using System.Domain.Entities;

namespace System.Infrastructure.Db
{
    public class DriveContext : DbContextBase<DriveContext>, IDbContextBase
    {
        public DriveContext(DbContextOptions<DriveContext> options, IEntityContext entityContext)
               : base(options, entityContext)
        {
        }

        public override string ContextName => "RPG Sessions";

        public DbSet<Blob> Container { get; set; } = default!;
        public DbSet<Metadata> Metadata { get; set; } = default!;
    }
}