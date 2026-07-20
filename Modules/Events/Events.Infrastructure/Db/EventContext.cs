using Base;
using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Db
{
    public class EventContext : DbContextBase<EventContext>, IDbContextBase
    {
        public EventContext(DbContextOptions<EventContext> options, IEntityContext entityContext)
        : base(options, entityContext)
        {
        }

        public override string ContextName => "Events";

        public DbSet<Event> Events { get; set; } = default!;
    }
}
