using Base;

namespace System.Infrastructure.JobEngine.Milestones
{
    public class MilestoneJob : IJob
    {
        public MilestoneJob(string title)
        {
            Name = $"[MILESTONE] {title}";
        }

        public int OperationId => 0;

        public Guid Id { get; set; }

        public List<IJob> Children => new();

        public DateTimeOffset RequestDate => DateTimeOffset.MaxValue;

        public string Name { get; private set; }

        public Task Execute(IJobContext jobContext)
        {
            return Task.CompletedTask;
        }
    }
}