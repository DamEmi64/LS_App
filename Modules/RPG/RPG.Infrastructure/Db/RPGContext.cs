using Base;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RPG.Domain.Entities;

namespace System.Infrastructure.Db
{
    public class RPGContext : DbContextBase<RPGContext>, IDbContextBase
    {
        public RPGContext(DbContextOptions<RPGContext> options, IEntityContext entityContext)
        : base(options, entityContext)
        {
        }

        public override string ContextName => "RPG Sessions";

        public DbSet<Story> Stories { get; set; } = default!;
        public DbSet<Chapter> Chapters { get; set; } = default!;
        public DbSet<Place> Places { get; set; } = default!;
        public DbSet<Hero> Heroes { get; set; } = default!;
    }
}