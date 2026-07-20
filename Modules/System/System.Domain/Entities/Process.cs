using Base;

namespace System.Domain.Entities
{
    public class Process : Entity
    {
        public required string Title { get; set; }
        public List<Job> Jobs { get; set; } = new List<Job>();

        public required DateTimeOffset RequestDate { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public double Percentage { get; set; }
        public ProgressStatus Status { get; set; }
        public UserData? User { get; set; }
        public string? TempData { get; set; }
        public List<ProcessError> Errors { get; set; } = new List<ProcessError>();

        public Job? GetJob(Guid id)
        {
            return Jobs.FirstOrDefault(x => x.Id == id);
        }
    }
}