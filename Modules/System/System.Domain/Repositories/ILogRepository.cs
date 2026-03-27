using System.Domain.Entities;

namespace System.Domain.Repositories
{
    public interface ILogRepository
    {
        IEnumerable<Log> GetAll();
        void RemoveLogs(IEnumerable<Log> logs);
    }
}