using Automation.Domain.Entities;
using Automation.Domain.Repositories;
using Automation.Infrastructure.Context;
using Base;
using Microsoft.EntityFrameworkCore;

namespace Automation.Infrastructure.Repositories
{
    public class AutomatRepository : EntityRepository<AutomationContext, Automat>, IAutomatRepository
    {
        public AutomatRepository(AutomationContext dbContext) : base(dbContext)
        {
        }

        public override Task<Automat?> Get(Guid id)
        {
            return DbContext.Set<Automat>().Include(x => x.Tasks).Include(x => x.Triggers).FirstOrDefaultAsync(x => x.Id == id);
        }

        public override IEnumerable<Automat> GetAll()
        {
            return DbContext.Set<Automat>().Include(x => x.Tasks).Include(x => x.Triggers);
        }

        public IEnumerable<Automat> TriggeredByEvent(params int[] eventIds)
        {
            return DbContext.Set<Automat>().Include(x => x.Triggers).Where(x => x.Triggers.Any(y => y.EventId.Any(z => eventIds.Contains(z))));
        }
    }
}
