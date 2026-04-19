using Base;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Domain.Repositories;

namespace System.Infrastructure.JobEngine
{
    public class JobContext : IJobContext
    {
        public JobContext(Guid id,
            IServiceProvider serviceProvider,
            string jobId,
            Guid processId)
        {
            Id = id;
            ServiceProvider = serviceProvider;
            JobId = jobId;
            ProcessId = processId;
        }

        public Guid Id { get; set; }

        public string JobId { get; private set; } = string.Empty;

        public Guid ProcessId { get; private set; }

        public IServiceProvider ServiceProvider { get; set; }

        public Task AddLog(string log)
        {
            var notifiers = ServiceProvider.GetServices<INotifier>();
            return Task.WhenAll((notifiers.Select(n => n.Process(NotifyTypes.Log, log))));
        }

        public async Task AddError(string error)
        {
            var repo = ServiceProvider.GetRequiredService<IProcessRepository>();
            await repo.AddError(ProcessId, JobId, error);
        }

        public static JobContext GetContext(IServiceProvider serviceProvider, Guid id, Guid processId, string jobId)
            => new(
                id,
                serviceProvider,
                jobId,
                processId
            );

        public string Data { get; set; } = string.Empty;

        public void PassData<T>(T data)
        {
            Data = JsonConvert.SerializeObject(data);
        }

        public T? GetData<T>()
        {
            if (Data is null)
                return default(T);

            return JsonConvert.DeserializeObject<T>(Data);
        }

        public T Resolve<T>(object? key = null)
            => key is null
            ? ServiceProvider.GetService<T>() ?? throw new ServiceNotRegistredException<T>()
            : ServiceProvider.GetKeyedService<T>(key) ?? throw new ServiceNotRegistredException<T>();
    }
}