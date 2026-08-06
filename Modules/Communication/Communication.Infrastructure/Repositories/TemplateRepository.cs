using Base;
using Communication.Domain.Entities;
using Communication.Domain.Repositories;
using Communication.Infrastructure.Db;

namespace Communication.Infrastructure.Repositories
{
    public class TemplateRepository : EntityRepository<CommunicationContext, Template>, ITemplateRepository
    {
        public TemplateRepository(CommunicationContext dbContext) : base(dbContext)
        {
        }
    }
}