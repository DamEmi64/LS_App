using Base;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Domain.Entities;

namespace System.Infrastructure.Db
{
    public class SystemContext : IdentityDbContext<User>, IDbContextBase
    {
        private readonly IEntityContext _entityContext;

        public SystemContext(DbContextOptions<SystemContext> options, IEntityContext entityContext)
        : base(options)
        {
            _entityContext = entityContext;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Process).Assembly);
        }

        public string ContextName => "(default)";

        public DbSet<DictionaryItem> Dictionaries { get; set; } = default!;
        public DbSet<Process> Processes { get; set; } = default!;
        public DbSet<ProcessError> ProcessErrors { get; set; } = default!;
        public DbSet<Job> Jobs { get; set; } = default!;
        public DbSet<ProcessMilestone> Milestones { get; set; } = default!;

        public override int SaveChanges()
        {
            ApplyUserInfo();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyUserInfo();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyUserInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ApplyUserInfo();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void ApplyUserInfo()
        {
            try
            {
                var entries = ChangeTracker.Entries<Entity>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

                foreach (var entry in entries)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.InsDate = DateTimeOffset.Now;
                        entry.Entity.UpdDate = DateTimeOffset.Now;
                        entry.Entity.InsBy = _entityContext.Editor;
                        entry.Entity.UpdBy = _entityContext.Editor;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entry.Entity.UpdDate = DateTimeOffset.Now;
                        entry.Entity.UpdBy = _entityContext.Editor;
                    }
                }
            }
            catch
            {
                // swallow any exceptions to avoid breaking SaveChanges in non-HTTP contexts
            }
        }
    }
}