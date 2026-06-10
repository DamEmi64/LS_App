using Base;
using Communication.Domain.Entities;
using Communication.Domain.Repositories;
using Communication.Infrastructure.Db;
using System;
using System.Collections.Generic;
using System.Text;

namespace Communication.Infrastructure.Repositories
{
    public class CommunicationHistoryRepository : EntityRepository<CommunicationContext, CommunicationHistory>, ICommunicationHistoryRepository
    {
        public CommunicationHistoryRepository(CommunicationContext dbContext) : base(dbContext)
        {
        }
    }
}
