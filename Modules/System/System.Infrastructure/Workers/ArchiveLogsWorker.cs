using Base;
using System.ComponentModel;
using System.Domain.Repositories;

namespace System.Infrastructure.Workers
{
    public class ArchiveLogsWorker : IWorker
    {
        private readonly ILogRepository _logRepository;
        public ArchiveLogsWorker(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public string Cron => Hangfire.Cron.Weekly();

        [DisplayName("[WORKER] Log Cleaner")]
        public void Execute()
        {
            var date = DateTime.Now.AddDays(-7);
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
