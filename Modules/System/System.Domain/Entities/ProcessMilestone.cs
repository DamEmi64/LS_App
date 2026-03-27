using Base;

namespace System.Domain.Entities
{
    public class ProcessMilestone : Entity
    {
        public Guid ProcessId { get; set; }
        public List<Guid> VerifyJobIds { get; set; } = new List<Guid>();
        public Guid JobId { get; set; }
        public bool Completed { get; set; }
    }
}