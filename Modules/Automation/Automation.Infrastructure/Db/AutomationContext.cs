using Automation.Domain.Entities;
using Base;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Automation.Infrastructure.Context
{
    public class AutomationContext : DbContextBase<AutomationContext>, IDbContextBase
    {
        public AutomationContext(DbContextOptions<AutomationContext> options, IEntityContext entityContext)
        : base(options, entityContext)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutomationContext).Assembly);
        }

        public override string ContextName => "Automation";

        public DbSet<Automat> Automats { get; set; } = default!;
        public DbSet<Domain.Entities.Task> AutomatTasks { get; set; } = default!;
        public DbSet<Domain.Entities.Trigger> Triggers { get; set; } = default!;
    }
}
