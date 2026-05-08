using Base;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Domain.Repositories;

namespace System.Infrastructure.Workers
{
    public class ArchiveLogsWorker
    {
        private readonly ILogRepository _logRepository;
        public ArchiveLogsWorker(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        [DisplayName("[LOG CLEANER]  {0}")]
        public void Execute(DateTime date)
        {
            var logsToDelete = _logRepository.GetAll().Where(x => x.TimeStamp < date);

            var archiveFolder = AppConfiguration.Get("LogArchiveFolder");

            if (!string.IsNullOrEmpty(archiveFolder.Value))
            {
                var dayLogs = logsToDelete.GroupBy(x => x.TimeStamp.Date.ToShortDateString());

                foreach (var dayLog in dayLogs)
                {
                    var filePath = Path.Combine(archiveFolder.Value, $"logs_{dayLog.Key}.txt");
                    using (var writer = new StreamWriter(filePath, true))
                    {
                        foreach (var log in dayLog)
                        {
                            writer.WriteLine($"[{log.Level}] {log.TimeStamp}: {log.Message}");
                        }
                    }
                }
            }

            _logRepository.RemoveLogs(logsToDelete);
        }
    }
}
