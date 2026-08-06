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
    }
}
