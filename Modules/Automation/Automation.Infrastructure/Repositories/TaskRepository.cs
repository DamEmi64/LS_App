using Automation.Domain.Repositories;
using Automation.Infrastructure.Context;
using Base;

namespace Automation.Infrastructure.Repositories
{
    public class TaskRepository : EntityRepository<AutomationContext, Domain.Entities.Task>, ITaskRepository
    {
        public TaskRepository(AutomationContext dbContext) : base(dbContext)
        {
        }
    }
}
