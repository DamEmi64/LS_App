using Automation.Domain.Entities;
using Automation.Domain.Repositories;
using Automation.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Automation.Infrastructure.Repositories
{
    public class SystemAutomateRepository : ISystemAutomateRepository
    {
        private readonly AutomationContext _automationContext;

        public SystemAutomateRepository(AutomationContext automationContext)
        {
            _automationContext = automationContext;
        }

        public async System.Threading.Tasks.Task Add(SystemAutomat entity)
        {
            await _automationContext.SystemAutomats.AddAsync(entity);
        }

        public async Task<SystemAutomat?> Get(int key)
        {
            return await _automationContext.SystemAutomats.FirstOrDefaultAsync(x => x.Key == key);
        }

        public async Task<SystemAutomat?> Get(Guid id)
        {
            return await _automationContext.SystemAutomats.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IEnumerable<SystemAutomat> GetAll()
        {
            return _automationContext.SystemAutomats;
        }

        public async System.Threading.Tasks.Task Remove(int key)
        {
            var automat = await Get(key);
            if (automat is not null)
                _automationContext.SystemAutomats.Remove(automat);
        }

        public System.Threading.Tasks.Task SaveChanges() => _automationContext.SaveChangesAsync();
    }
}
