using Base;
using System.Domain.Entities;
using System.Infrastructure.JobEngine.Milestones;

namespace System.Infrastructure.JobEngine
{
    public class ProcessSchema : IProcessSchema
    {
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

        public Process Process { get; set; }
        public List<IJob> Jobs { get; set; }
        public List<(string title, IJob[] jobs, IJob current)> Milestones { get; set; }
        public IJob? Job { get; set; }

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
        private ProcessJobSchema? _parent;

        public ProcessJobSchema(IJob job, ProcessJobSchema? parent, Process process, List<IJob> jobs, List<(string title, IJob[] jobs, IJob current)> milestones)
            : base(process, jobs, milestones)
        {
            _parent = parent;
            Job = job;
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

            if (_parent?.Job is null)
            {
                Jobs.Add(job);
            }
            else
            {
                _parent.Job.Children.Add(job);
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
                RequestDate = job.RequestDate,
                OperationId = job.OperationId
            };

            job.Id = entity.Id;

            Process.Jobs.Add(entity);
            ArgumentNullException.ThrowIfNull(Job);
            Job.Children.Add(job);

            return new ProcessJobSchema(job, this, Process, Jobs, Milestones);
        }

        public IProcessJobSchema? FindLastJobByOperation(int operation)
        {
            ArgumentNullException.ThrowIfNull(Job);
            if (Job.OperationId == operation)
            {
                return this;
            }

            var current = _parent;

            while (true)
            {nnnnnnn
                if (current is null)
                {
                    return null;
                }

                if (current.Job is null)
                {
                    return null;
                }

                if (current.Job.OperationId == operation)
                {
                    return current;
                }

                current = current._parent;

            }
        }
    }
}