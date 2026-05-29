using Base;
using Newtonsoft.Json;
using System.Domain.Entities;
using System.Domain.Repositories;
using System.Infrastructure.Services.EntityContext;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace System.Infrastructure.JobEngine
{
    public class JobContext : IJobContext
    {
        private readonly INotifier _notifier;
        private readonly IProcessRepository _processRepository;
        private readonly IJobRepository _jobRepository;
        private readonly EntityContext _entityContext;

        public JobContext(INotifier notifier,
            IProcessRepository processRepository,
            IEntityContext entityContext,
            IJobRepository jobRepository)
        {
            _notifier = notifier;
            _processRepository = processRepository;
            _entityContext = entityContext as EntityContext ?? throw new InvalidCastException("Invalid entity context");
            _jobRepository = jobRepository;
        }

        public Guid Id { get; set; }

        public string JobId { get; private set; } = string.Empty;

        public Guid ProcessId { get; private set; }

        public Task AddLog(string log) => _notifier.Process(NotifyTypes.Log, log);

        public async Task AddError(string error)
        {
            await _notifier.ProcessError(NotifyTypes.ProcessError, error);
            await _processRepository.AddError(ProcessId, JobId, error);
        }

        public void Setup(Guid id, Guid processId, string jobId)
        {
            ProcessId = processId;
            Id = id;
            JobId = jobId;
        }

        public string Data { get; set; } = string.Empty;

        public void PassData<T>(T data)
        {
            var process = _processRepository.Get(ProcessId).Result;
            if (process != null)
                process.TempData = JsonConvert.SerializeObject(data);
        }

        public T? GetData<T>()
        {
            var process = _processRepository.Get(ProcessId).Result;
            if (process != null)
                return JsonConvert.DeserializeObject<T>(process.TempData ?? string.Empty);

            return default;
        }

        public async Task OnStart()
        {
            _entityContext.Process(ProcessId);
            var process = await _processRepository.Get(ProcessId);
            ArgumentNullException.ThrowIfNull(process);

            if (process.Status == ProgressStatus.New)
            {
                process.StartDate = DateTime.Now;
                await _processRepository.Update(process);
            }

            if (process.Status != ProgressStatus.Executing)
            {
                process.Status = ProgressStatus.Executing;
                await _processRepository.Update(process);
            }

            var dbJob = process.GetJob(Id);

            if (dbJob is null)
            {
                process.Status = ProgressStatus.Success;
                process.EndDate = DateTimeOffset.Now;
                await _processRepository.Update(process);
                return;
            }

            dbJob.StartDate = DateTimeOffset.Now;
            dbJob.JobId = JobId;
            dbJob.Status = ProgressStatus.Executing;
            await _jobRepository.Update(dbJob);
        }

        public async Task OnComplete()
        {
            var dbJob = await _jobRepository.Get(Id);
            var process = await _processRepository.Get(ProcessId);
            ArgumentNullException.ThrowIfNull(dbJob);
            ArgumentNullException.ThrowIfNull(process);

            dbJob.EndDate = DateTimeOffset.Now;
            dbJob.Status = ProgressStatus.Success;
            await _jobRepository.Update(dbJob);
            process.Percentage = (process.Jobs.Count(x => x.Status == ProgressStatus.Success) * 1.0 / process.Jobs.Count) * 100;

            if (process.Percentage == 100)
            {
                await EndProcess(process);
                return;
            }
            else
            {
                await _processRepository.Update(process);
            }
        }

        private Task EndProcess(Process process)
        {
            process.TempData = string.Empty;
            process.Status = ProgressStatus.Success;
            process.EndDate = DateTimeOffset.Now;

            if (process.Errors.Any())
            {
                process.Status = ProgressStatus.Failed;
            }

            return _processRepository.Update(process);
        }
    }
}