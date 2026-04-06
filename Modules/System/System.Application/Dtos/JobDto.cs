using Base;

namespace System.Application.Dtos
{
    public class JobDto
    {
        public required string Name { get; set; }
        public string? JobId { get; set; }
        public ProgressStatus Status { get; set; }
        public DateTimeOffset RequestDate { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public Guid Process { get; set; }
        public Guid? Parent { get; set; }
        public List<JobDto> Children { get; set; } = new List<JobDto>();
        public string? JobData { get; set; }
        public int Operation { get; set; }
    }
}