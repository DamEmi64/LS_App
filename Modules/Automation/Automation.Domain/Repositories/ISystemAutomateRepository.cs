using SystemAutomat =  Automation.Domain.Entities.SystemAutomat;

namespace Automation.Domain.Repositories
{
    public interface ISystemAutomateRepository
    {
        Task Add(SystemAutomat entity);
        IEnumerable<SystemAutomat> GetAll();
        Task<SystemAutomat?> Get(int key);
        Task<SystemAutomat?> Get(Guid id);
        Task Remove(int key);
        Task SaveChanges();
    }
}
