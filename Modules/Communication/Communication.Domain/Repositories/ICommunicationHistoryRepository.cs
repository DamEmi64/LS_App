using Base;
using Communication.Domain.Entities;

namespace Communication.Domain.Repositories
{
    public interface ICommunicationHistoryRepository : IEntityRepository<CommunicationRegistry>
    {
        IEnumerable<CommunicationRegistry> GetCommunicationHistoryByDateRange(DateTime startDate, DateTime endDate);
        void DeleteLogs(IEnumerable<CommunicationRegistry> logs);
    }
}
