using Base;
using Communication.Domain.Entities;
using Communication.Infrastructure.Db;
using Communication.Domain.Repositories;

namespace Communication.Infrastructure.Repositories
{
    public class TemplateRepository : EntityRepository<CommunicationContext, Template>, ITemplateRepository
    {
        public TemplateRepository(CommunicationContext dbContext) : base(dbContext)
        {
        }
    }
}