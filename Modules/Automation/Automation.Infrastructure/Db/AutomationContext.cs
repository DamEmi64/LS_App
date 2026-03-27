using Automation.Domain.Entities;
using Base;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Automation.Infrastructure.Context
{
    public class AutomationContext : DbContextBase<AutomationContext>, IDbContextBase
    {
        public AutomationContext(DbContextOptions<AutomationContext> options, IEntityContext entityContext)
        : base(options, entityContext)
        {
        }

        public override string ContextName => "Automation";

        public DbSet<Automat> Automats { get; set; } = default!;
        public DbSet<Domain.Entities.Task> AutomatTasks { get; set; } = default!;
    }
}
