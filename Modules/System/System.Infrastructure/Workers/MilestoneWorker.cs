using Hangfire;
using System.ComponentModel;
using System.Infrastructure.JobEngine;
using System.Domain.Repositories;
using Base;

namespace System.Infrastructure.Workers
{
    public class MilestoneWorker : IWorker
    {
        private readonly IProcessRepository _processRepository;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public MilestoneWorker(IBackgroundJobClient backgroundJobClient, IProcessRepository processRepository)
        {
            _backgroundJobClient = backgroundJobClient;
            _processRepository = processRepository;
        }

        public string Cron => Hangfire.Cron.MinuteInterval(15);

        [DisplayName("[MILESTONE CHECKER]")]
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

                    using (AmbientTransaction.Suppress())
                    {
                        _backgroundJobClient.Reschedule(jobId, DateTimeOffset.Now);
                    }
                }
            }
        }
    }
}
