using Base;
using Communication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastructure.Db
{
    public class CommunicationContext : DbContextBase<CommunicationContext>, IDbContextBase
    {
        public CommunicationContext(DbContextOptions<CommunicationContext> options, IEntityContext entityContext)
        : base(options, entityContext)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Email).Assembly);
        }

        public override string ContextName => "Communication";

        public DbSet<UserData> EmailUsers { get; set; } = default!;
        public DbSet<Email> Emails { get; set; } = default!;
        public DbSet<Template> Templates { get; set; } = default!;
    }
}