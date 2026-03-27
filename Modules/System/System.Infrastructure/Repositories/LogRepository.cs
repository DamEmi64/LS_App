using System.Domain.Entities;
using System.Domain.Repositories;
using System.Infrastructure.Db;

namespace System.Infrastructure.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly ErrorContext _errorContext;

        public LogRepository(ErrorContext errorContext)
        {
            _errorContext = errorContext;
        }

        public IEnumerable<Log> GetAll()
        {
            return _errorContext.Logs;
        }

        public void RemoveLogs(IEnumerable<Log> logs)
        {
            _errorContext.Logs.RemoveRange(logs);
            _errorContext.SaveChanges();
        }
    }
}