using Base;
using System.Domain.Entities;
namespace System.Application.Dtos
{
    public class ProcessErrorDto
    {
        public int Id { get; set; }
        public string? JobId { get; set; }
        public string? Message { get; set; }
        public static ProcessErrorDto ToDto(ProcessError entity) => new() { Id = Random.Shared.Next(), JobId = entity.JobId, Message = entity.Message };
    }
}
