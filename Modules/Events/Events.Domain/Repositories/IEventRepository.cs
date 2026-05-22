using Base;
using Events.Domain.Entities;

namespace Events.Domain.Repositories
{
    public interface IEventRepository : IEntityRepository<Event>
    {
        IEnumerable<Event> GetByUser(string userId);
        Event? GetLastAdded();
        Task SignIn(EventUser user);
    }
}
