using Base;
using Hangfire.Server;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Domain.Entities;
using System.Domain.Repositories;
using System.Infrastructure.Services.EntityContext;

namespace System.Infrastructure.JobEngine
{
    public interface IJobExecutor
    {
        void Execute(string title, IJob job, Guid processId, PerformContext? context);
    }

    public class JobExecutor : IJobExecutor
    {
        private readonly IProcessRepository _processRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly List<INotifier> _notifiers;
        private readonly EntityContext _entityContext;

        public JobExecutor(IProcessRepository processRepository,
            IJobRepository jobRepository,
            IServiceProvider serviceProvider)
        {
            _processRepository = processRepository;
            _jobRepository = jobRepository;
            _serviceProvider = serviceProvider;
            _notifiers = serviceProvider.GetServices<INotifier>().ToList();
            _entityContext = serviceProvider.GetRequiredService<IEntityContext>() as EntityContext ?? throw new NullReferenceException();
        }

        public void Execute(string title, IJob job, Guid processId, PerformContext? context)
        {
            ArgumentNullException.ThrowIfNull(context);
            Task.Run(() => ExecuteInternal(job, processId, context)).Wait(CancellationToken.None);
        }

        private async Task ExecuteInternal(IJob job, Guid processId, PerformContext context)
        {
            _entityContext.Process(processId);
            var process = await _processRepository.Get(processId);
            ArgumentNullException.ThrowIfNull(process);

            if (process.Status == ProgressStatus.New)
            {
                process.StartDate = DateTime.Now;
                await _processRepository.Update(process);
                await Notify(NotifyTypes.ProcessStart, process.Title);
            }

            if (process.Status != ProgressStatus.Executing)
            {
                process.Status = ProgressStatus.Executing;
                await _processRepository.Update(process);
            }

            var dbJob = process.GetJob(job.Id);

            if (dbJob is null)
            {
                process.Status = ProgressStatus.Success;
                process.EndDate = DateTimeOffset.Now;
                await _processRepository.Update(process);
                await Notify(NotifyTypes.ProcessCompleted, process.Title);
                return;
            }

            JobContext jobContext;

            try
            {
                dbJob.StartDate = DateTimeOffset.Now;
                dbJob.JobId = context.BackgroundJob.Id;
                dbJob.Status = ProgressStatus.Executing;
                await _jobRepository.Update(dbJob);

                jobContext = JobContext.GetContext(_serviceProvider, dbJob.Id, process.Id, context.BackgroundJob.Id);
                jobContext.Data = process.TempData ?? string.Empty;
                try
                {
                    await job.Execute(jobContext);

                    dbJob.EndDate = DateTimeOffset.Now;
                    dbJob.Status = ProgressStatus.Success;
                    await _jobRepository.Update(dbJob);
                    process.TempData = jobContext.Data;
                    process.Percentage = (process.Jobs.Where(x => x.Status == ProgressStatus.Success).Count() * 1.0 / process.Jobs.Count) * 100;

                    if (process.Percentage == 100)
                    {
                        await EndProcess(process);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    process.Errors.Add(new ProcessError
                    {
                        Id = Guid.Empty,
                        InsDate = DateTimeOffset.UtcNow,
                        JobId = dbJob.JobId ?? "--",
                        Message = ex.Message,
                        UpdDate = DateTimeOffset.UtcNow
                    });

                    await _processRepository.Update(process);
                    throw;  
                }

                if (process.Errors.Any())
                {
                    process.Status = ProgressStatus.Failed;
                    await _processRepository.Update(process);
                    return;
                }

                process.Status = ProgressStatus.Paused;
                await _processRepository.Update(process);
            }
            catch (Exception ex)
            {
                dbJob.Status = ProgressStatus.Failed;
                dbJob.EndDate = DateTimeOffset.Now;
                process.Status = ProgressStatus.Failed;
                await _jobRepository.Update(dbJob);
                await _processRepository.Update(process);
                await NotifyError(NotifyTypes.ProcessError, process.Title, ex.Message);
                throw;
            }
        }

        private Task Notify(int messageId, params object[] args)
        {
            return Task.WhenAll(_notifiers.Select(n => n.Process(messageId, args)));
        }

        private Task NotifyError(int messageId, params object[] args)
        {
            return Task.WhenAll(_notifiers.Select(n => n.ProcessError(messageId, args)));
        }

        private Task EndProcess(Process process)
        {
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