using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Base
{
    /// <summary>
    ///         Provides a base implementation of <see cref="DbContext"/> used in App
    /// </summary>
    /// <typeparam name="T">The concrete type of the derived DbContext.</typeparam>
    public abstract class DbContextBase<T> : DbContext, IDbContextBase where T : DbContext
    {
        private readonly IEntityContext _entityContext;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DbContextBase{T}"/> class.
        /// </summary>
        /// <param name="options">The options to configure the context.</param>
        /// <param name="entityContext">Entity database context.</param>
        public DbContextBase(DbContextOptions<T> options, IEntityContext entityContext) : base(options)
        {
            _entityContext = entityContext;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(T).Assembly);
        }

        public abstract string ContextName { get; }

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
                var entries = ChangeTracker.Entries<Entity>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

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
                // Intentionally ignored to prevent audit logic from breaking SaveChanges
            }
        }
    }
}