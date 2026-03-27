using Base;
using System.Domain.Entities;
using System.Infrastructure.JobEngine.Milestones;

namespace System.Infrastructure.JobEngine
{
    public class ProcessSchema : IProcessSchema
    {
        public Process Process { get; set; }
        public List<IJob> Jobs { get; set; }
        public List<(string title, IJob[] jobs, IJob current)> Milestones { get; set; }

        public ProcessSchema(string title)
        {
            Process = new Process
            {
                Status = ProgressStatus.New,
                Jobs = new List<Job>(),
                Title = title
            };

            Jobs = new();
            Milestones = new();
        }

        public ProcessSchema(Process process, List<IJob> jobs, List<(string title, IJob[] jobs, IJob current)> milestones)
        {
            Process = process;
            Jobs = jobs;
            Milestones = milestones;
        }

        public virtual IProcessJobSchema AddJob(IJob job)
        {
            var entity = new Job
            {
                Id = Guid.NewGuid(),
                Name = job.Name,
                Status = ProgressStatus.New,
                InsDate = DateTime.Now,
                RequestDate = job.RequestDate,
                OperationId = job.OperationId
            };

            job.Id = entity.Id;

            Process.Jobs.Add(entity);
            Jobs.Add(job);

            return new ProcessJobSchema(job, null, Process, Jobs, Milestones);
        }

        public IProcessSchema AddMilestone(string title, params IJob[] jobs)
        {
            var job = new MilestoneJob(title);
            AddJob(job);
            Milestones.Add((title, jobs, job));
            return this;
        }

        public IProcessSchema AddMilestone(string title)
        {
            var job = new MilestoneJob(title);
            AddJob(job);
            Milestones.Add((title, Jobs.ToArray(), job));
            return this;
        }
    }

    public class ProcessJobSchema : ProcessSchema, IProcessJobSchema
    {
        private IJob? _parent;
        private IJob _current;

        public ProcessJobSchema(IJob current, IJob? parent, Process process, List<IJob> jobs, List<(string title, IJob[] jobs, IJob current)> milestones)
            : base(process, jobs, milestones)
        {
            _parent = parent;
            _current = current;
        }

        public override IProcessJobSchema AddJob(IJob job)
        {
            var entity = new Job
            {
                Id = Guid.NewGuid(),
                Name = job.Name,
                Status = ProgressStatus.New,
                InsDate = DateTime.Now,
                RequestDate = job.RequestDate,
                OperationId = job.OperationId
            };

            job.Id = entity.Id;

            Process.Jobs.Add(entity);

            if (_parent is null)
            {
                Jobs.Add(job);
            }
            else
            {
                _parent.Children.Add(job);
            }

            return this;
        }

        public IProcessJobSchema AddChildJob(IJob job)
        {
            var entity = new Job
            {
                Id = Guid.NewGuid(),
                Name = job.Name,
                Status = ProgressStatus.New,
                InsDate = DateTime.Now,
                RequestDate = job.RequestDate
            };

            job.Id = entity.Id;

            Process.Jobs.Add(entity);
            _current.Children.Add(job);

            return new ProcessJobSchema(job, _current, Process, Jobs, Milestones);
        }
    }
}