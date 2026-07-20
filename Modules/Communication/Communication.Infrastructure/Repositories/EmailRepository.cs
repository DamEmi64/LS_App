using Base;
using Communication.Domain.Entities;
using Communication.Domain.Repositories;
using Communication.Infrastructure.Db;

namespace Communication.Infrastructure.Repositories
{
    public class EmailRepository : EntityRepository<CommunicationContext, Email>, IEmailRepository
    {
        public EmailRepository(CommunicationContext dbContext) : base(dbContext)
        {
        }
    }
}