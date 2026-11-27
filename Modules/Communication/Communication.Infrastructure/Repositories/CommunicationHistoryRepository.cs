using Base;
using Communication.Domain.Entities;
using Communication.Domain.Repositories;
using Communication.Infrastructure.Db;

namespace Communication.Infrastructure.Repositories
{
    public class CommunicationHistoryRepository : EntityRepository<CommunicationContext, CommunicationRegistry>, ICommunicationHistoryRepository
    {
        public CommunicationHistoryRepository(CommunicationContext dbContext) : base(dbContext)
        {
        }

        public void DeleteLogs(IEnumerable<CommunicationRegistry> logs)
        {
            DbContext.CommunicationRegistry.RemoveRange(logs);
            DbContext.SaveChanges();
        }

        public IEnumerable<CommunicationRegistry> GetCommunicationHistoryByDateRange(DateTime startDate, DateTime endDate)
        {
           return DbContext.CommunicationRegistry.Where(c => c.InsDate >= startDate && c.InsDate <= endDate).ToList();
        }
    }
}
