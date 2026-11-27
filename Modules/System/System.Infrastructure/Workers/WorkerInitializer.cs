using Base;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace System.Infrastructure.Workers
{
    public static class WorkerInitializer
    {
        public static void InitializeWorkers(WebApplication app)
        {
            app.Logger.LogInformation($"Verify dictionaries...");
            using (var scope = app.Services.CreateScope())
            {
                var workers = scope.ServiceProvider.GetServices<IWorker>();

                foreach (var worker in workers)
                {
                    if (worker is null)
                        continue;

                    var workerType = worker.GetType();
                    var jobId = workerType.Name;
                    RecurringJob.AddOrUpdate(jobId, () => worker.Execute(), worker.Cron);
                }
            }
        }
    }
}
