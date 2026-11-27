using Base;
using System.Domain.Entities;
namespace System.Application.Dtos
{
    public class ProcessDto : Base.IDto<Process, ProcessDto>
    {
        public required string Title { get; set; }
        public string Queue { get; set; } = "default";
        public List<JobDto> Jobs { get; set; } = new();
        public List<ProcessErrorDto> Errors { get; set; } = new();
        public DateTimeOffset? RequestDate { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public double Percentage { get; set; }
        public ProgressStatus Status { get; set; }
        public UserData? User { get; set; }
        public Guid Id { get; set; }
        public DateTimeOffset InsDate { get; set; }
        public DateTimeOffset UpDate { get; set; }

        public Process ToEntity() => new()
        {
            Id = Id == Guid.Empty ? Guid.NewGuid() : Id, InsDate = Id == Guid.Empty ? DateTimeOffset.UtcNow : InsDate,
            UpdDate = DateTimeOffset.UtcNow, Title = Title, RequestDate = RequestDate ?? DateTimeOffset.UtcNow,
            StartDate = StartDate ?? DateTimeOffset.UtcNow, EndDate = EndDate ?? DateTimeOffset.UtcNow,
            Percentage = Percentage, Status = Status, User = User,
            Jobs = Jobs.Select(x => x.ToEntity()).ToList()
        };
        public static ProcessDto ToDto(Process entity) => new()
        {
            Id = entity.Id, InsDate = entity.InsDate, UpDate = entity.UpdDate, Title = entity.Title,
            RequestDate = entity.RequestDate, StartDate = entity.StartDate, EndDate = entity.EndDate,
            Percentage = entity.Percentage, Status = entity.Status, User = entity.User,
            Jobs = entity.Jobs.Select(x => JobDto.ToDto(x)).ToList(),
            Errors = entity.Errors.Select(ProcessErrorDto.ToDto).ToList()
        };
        public static ProcessDto ToDto(ProcessRead entity) => new()
        {
            Id = entity.Id, InsDate = entity.InsDate, UpDate = entity.UpdDate, Title = entity.Title,
            RequestDate = entity.RequestDate, StartDate = entity.StartDate, EndDate = entity.EndDate,
            Percentage = entity.Percentage, Status = entity.Status, User = entity.User,
            Jobs = entity.Jobs.Select(x => JobDto.ToDto(x)).ToList(),
            Errors = entity.Errors.Select(ProcessErrorDto.ToDto).ToList()
        };
        public static explicit operator Process(ProcessDto dto) => dto.ToEntity();
        public static explicit operator ProcessDto(Process entity) => ToDto(entity);
    }
}
