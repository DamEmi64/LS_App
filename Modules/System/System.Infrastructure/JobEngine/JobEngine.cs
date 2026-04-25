using Base;
using Hangfire;
using Hangfire.Server;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Domain.Entities;
using System.Domain.Repositories;

namespace System.Infrastructure.JobEngine
{
    public class JobEngine : IJobEngine
    {
        private readonly IProcessRepository _processRepository;
        private readonly UserManager<User> _userStore;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IJobExecutor _jobExecutor;
        private readonly IConnectorResolver _connector;

        public JobEngine(UserManager<User> userStore,
            IProcessRepository processRepository,
            IBackgroundJobClient backgroundJobClient,
            IJobExecutor jobExecutor,
            IConnectorResolver connector)
        {
            _userStore = userStore;
            _processRepository = processRepository;
            _backgroundJobClient = backgroundJobClient;
            _jobExecutor = jobExecutor;
            _connector = connector;
        }

        public IProcessSchema Create(string title) => new ProcessSchema(title);

        public async Task Execute(IProcessSchema schema, UserData userData)
        {
            Validate(schema, userData);

            var schemaObject = (ProcessSchema)schema;
            schemaObject.Process.User = userData.Clone();
            schemaObject.Process.Schema = JsonConvert.SerializeObject(schemaObject, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
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
                var operation = _connector.GetOperation(job.OperationId);
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
                    var operation = _connector.GetOperation(child.OperationId);
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
            _jobExecutor.Execute(title, job, processId, performContext);
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
    }
}