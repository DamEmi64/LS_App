using Base;

namespace System.Application.Dtos
{
    public class ProcessDto
    {
        public required string Title { get; set; }
        public string Queue { get; set; } = "default";
        public List<JobDto> Jobs { get; set; } = new List<JobDto>();
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public double Percentage { get; set; }
        public ProgressStatus Status { get; set; }
        public UserData? User { get; set; }
        public Guid Id { get; set; }
        public DateTimeOffset InsDate { get; set; }

        public DateTimeOffset UpDate { get; set; }
    }
}