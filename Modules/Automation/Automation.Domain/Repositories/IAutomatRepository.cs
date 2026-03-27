using Automation.Domain.Entities;
using Base;

namespace Automation.Domain.Repositories
{
    public interface IAutomatRepository : IEntityRepository<Automat>
    {
        IEnumerable<Automat> TriggeredByEvent(params int[] eventIds);
    }
}
