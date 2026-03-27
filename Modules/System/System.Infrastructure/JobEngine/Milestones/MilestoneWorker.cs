using Hangfire;
using System.Domain.Repositories;

namespace System.Infrastructure.JobEngine.Milestones
{
    public interface IMilestoneWorker
    {
        void Execute();
    }

    public class MilestoneWorker : IMilestoneWorker
    {
        private readonly IProcessRepository _processRepository;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public MilestoneWorker(IBackgroundJobClient backgroundJobClient, IProcessRepository processRepository)
        {
            _backgroundJobClient = backgroundJobClient;
            _processRepository = processRepository;
        }

        public void Execute()
        {
            foreach (var milestone in _processRepository.GetActiveMilestones())
            {
                if (_processRepository.CheckIfEnded(milestone.ProcessId, milestone.VerifyJobIds.ToArray()))
                {
                    var jobId = _processRepository.GetHangfireJobId(milestone.JobId);

                    if (jobId is null)
                    {
                        continue;
                    }

                    _backgroundJobClient.Reschedule(jobId, DateTimeOffset.Now);
                }
            }
        }
    }
}