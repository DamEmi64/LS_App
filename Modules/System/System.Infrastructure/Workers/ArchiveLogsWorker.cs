using Base;
using Microsoft.Extensions.DependencyInjection;
using System.Domain.Repositories;

namespace System.Infrastructure.Workers
{
    public class ArchiveLogsWorker
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var logRepo = serviceProvider.GetRequiredService<ILogRepository>();

            var logsToDelete = logRepo.GetAll().Where(x => x.TimeStamp < DateTime.Now.AddDays(-30));

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

            logRepo.RemoveLogs(logsToDelete);
        }
    }
}
