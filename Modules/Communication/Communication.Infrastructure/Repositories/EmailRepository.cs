using Base;
using Communication.Domain.Entities;
using Communication.Infrastructure.Db;
using Files.Domain.Repositories;

namespace Communication.Infrastructure.Repositories
{
    public class EmailRepository : EntityRepository<CommunicationContext, Email>, IEmailRepository
    {
        public EmailRepository(CommunicationContext dbContext) : base(dbContext)
        {
        }
    }
}