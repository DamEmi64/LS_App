using Base;
using Events.Domain.Entities;
using Events.Domain.Repositories;
using Events.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;


namespace Events.Infrastructure.Repositories
{
    public class EventRepository : EntityRepository<EventContext, Event>, IEventRepository
    {
        public EventRepository(EventContext dbContext)
            : base(dbContext)
        {
        }

        public override Task<Event?> Get(Guid id)
        {
            return DbContext.Set<Event>().Include(x => x.Participates).FirstOrDefaultAsync(x => x.Id == id);
        }

        public override IEnumerable<Event> GetAll()
        {
            return DbContext.Set<Event>().Include(x => x.Participates);
        }

        public IEnumerable<Event> GetByUser(string userId)
        {
            return DbContext.Set<Event>().Include(x => x.Participates).Where(x => x.Participates.Any(x => x.UserId == userId));
        }

        public async Task SignIn(EventUser user)
        {
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();
        }
        public Event? GetLastAdded()
        {
            return GetAll().OrderByDescending(x => x.InsDate).FirstOrDefault();
        }
    }
}
