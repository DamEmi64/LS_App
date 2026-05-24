using Base;
using Hangfire;
using Hangfire.Server;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.Domain.Entities;
using System.Domain.Repositories;
using System.Infrastructure.Services.ConnectorResolver;

namespace System.Infrastructure.JobEngine
{
    public class JobEngine : IJobEngine
    {
        private readonly IProcessRepository _processRepository;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IConnectorService _connectorService;
        private readonly IJobContext _jobContext;
        private readonly IConnect _connect;

        public JobEngine(
            IProcessRepository processRepository,
            IBackgroundJobClient backgroundJobClient,
            IConnect connect,
            IConnectorService connectorService,
            IJobContext jobContext)
        {
            _processRepository = processRepository;
            _backgroundJobClient = backgroundJobClient;
            _connect = connect;
            _connectorService = connectorService;
            _jobContext = jobContext;
        }

        public IProcessSchema Create(string title) => new ProcessSchema(title);

        public async Task<Guid> Execute(IProcessSchema schema, UserData userData)
        {
            Validate(schema, userData);

            var schemaObject = (ProcessSchema)schema;
            schemaObject.Process.User = userData.Clone();
            await _processRepository.Add(schemaObject.Process);
            var root = ScheduleJobs(schemaObject.Process, schemaObject.Jobs);
            await _processRepository.Update(schemaObject.Process);
            await _processRepository.AddMilestones(schemaObject.Milestones.Select(x => new ProcessMilestone
            {
                Id = Guid.NewGuid(),
                Completed = false,
                InsDate = DateTimeOffset.Now,
                UpdDate = DateTimeOffset.Now,
                ProcessId = schemaObject.Process.Id,
                JobId = x.current.Id,
                VerifyJobIds = x.jobs.Select(j => j.Id).ToList(),
            }));

            _backgroundJobClient.Reschedule(root, DateTimeOffset.Now.AddSeconds(1));

            return schemaObject.Process.Id;
        }

        private void Validate(IProcessSchema schema, UserData userData)
        {
            ArgumentNullException.ThrowIfNull(userData.Email, "User Data");

            if (schema is not ProcessSchema)
                throw new InvalidOperationException("Cannot convert schema");
        }

        private string ScheduleJobs(Process process, List<IJob> jobs)
        {
            var stack = new Stack<(List<IJob>, string)>();

            var root = _backgroundJobClient.Schedule(() => ProcessStart(process.Title), DateTimeOffset.MaxValue);
            foreach (var job in jobs)
            {
                var operation = _connectorService.GetOperation(job.OperationId);
                ArgumentNullException.ThrowIfNull(operation, $"Operation {job.OperationId} not found");
                var taskname = $"[{process.Id}:{process.Title}] {job.Name}";
                var jobId = _backgroundJobClient.ContinueJobWith(root, operation.Queue, () => ExecuteJob(taskname, job, process.Id, null));
                SetJobId(process, job, jobId);
                stack.Push((job.Children, jobId));
            }

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                foreach (var child in current.Item1)
                {
                    var operation = _connectorService.GetOperation(child.OperationId);
                    ArgumentNullException.ThrowIfNull(operation, $"Operation {child.OperationId} not found");
                    var taskname = $"({process.Title}) {child.Name}";

                    var jobId = _backgroundJobClient.ContinueJobWith(current.Item2, operation.Queue, () => ExecuteJob(taskname, child, process.Id, null));
                    SetJobId(process, child, jobId);
                    stack.Push((child.Children, jobId));
                }
            }

            return root;
        }

        [DisplayName("{0}")]
        public void ExecuteJob(string title, IJob job, Guid processId, PerformContext? performContext)
        {
            ArgumentNullException.ThrowIfNull(performContext);
            ExecuteAsync(job, processId, performContext).Wait();
        }

        private async Task ExecuteAsync(IJob job, Guid processId, PerformContext performContext)
        {
            if (_jobContext is JobContext jobContext)
            {
                jobContext.Setup(job.Id, processId, performContext.BackgroundJob.Id);
                _ = await _connect.Send(job);
            }
            else
            {
                throw new InvalidCastException("Invalid job context");
            }
        }

        private void SetJobId(Process process, IJob job, string jobId)
        {
            var dbJob = process.GetJob(job.Id);
            if (dbJob is not null)
            {
                dbJob.JobId = jobId;
            }
        }

        [DisplayName("[PROCESS START] {0}")]
        public void ProcessStart(string title)
        {
        }

        public async Task Cancel(Guid processId)
        {
            var process = await _processRepository.Get(processId);
            ArgumentNullException.ThrowIfNull(process);
            foreach (var job in process.Jobs)
            {
                _backgroundJobClient.Delete(job.JobId);
            }
        }
    }
}