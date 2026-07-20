using Base;
using Communication.Domain.Entities;

namespace Communication.Domain.Repositories
{
    public interface ICommunicationHistoryRepository : IEntityRepository<CommunicationRegistry>
    {
    }
}
