using Base;

namespace System.Domain.Entities
{
    public class Job : Entity
    {
        public required string Name { get; set; }
        public string? JobId { get; set; }
        public int OperationId { get; set; }
        public ProgressStatus Status { get; set; }
        public DateTimeOffset RequestDate { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public Process Process { get; set; } = default!;
        public Job? Parent { get; set; }
        public List<Job> Children { get; set; } = new List<Job>();
        public JobData? JobData { get; set; }
    }
}