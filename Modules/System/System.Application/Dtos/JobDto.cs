using Base;
using System.Domain.Entities;
namespace System.Application.Dtos
{
    public class JobDto :IDto<Job, JobDto>
    {
        public required string Name { get; set; }
        public string? JobId { get; set; }
        public ProgressStatus Status { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public Guid Process { get; set; }
        public Guid? Parent { get; set; }
        public List<JobDto> Children { get; set; } = new();
        public string? JobData { get; set; }
        public int Operation { get; set; }
        public Job ToEntity() => new()
        {
            Name = Name, JobId = JobId, Status = Status, StartDate = StartDate ?? DateTimeOffset.UtcNow,
            EndDate = EndDate, OperationId = Operation,
            Process = Process == Guid.Empty ? null! : new Process { Id = Process, Title = Name, RequestDate = DateTimeOffset.UtcNow },
            Parent = Parent.HasValue ? new Job { Id = Parent.Value, Name = Name } : null,
            Children = Children.Select(x => x.ToEntity()).ToList(),
            JobData = JobData is null ? null : new JobData { JsonData = JobData }
        };
        public static JobDto ToDto(Job entity, int depth = 0) => new()
        {
            Name = entity.Name, JobId = entity.JobId, Status = entity.Status, StartDate = entity.StartDate,
            EndDate = entity.EndDate, Process = entity.Process?.Id ?? Guid.Empty, Parent = entity.Parent?.Id,
            Operation = entity.OperationId, JobData = entity.JobData?.JsonData,
            Children = depth >= 2 ? new() : entity.Children.Select(x => ToDto(x, depth + 1)).ToList()
        };

        public static JobDto ToDto(Job entity) => ToDto(entity, 0);

        public static explicit operator Job(JobDto dto) => dto.ToEntity();
        public static explicit operator JobDto(Job entity) => ToDto(entity);
    }
}
