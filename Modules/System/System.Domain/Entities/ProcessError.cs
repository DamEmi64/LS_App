using Base;

namespace System.Domain.Entities
{
    public class ProcessError : Entity
    {
        public required string JobId { get; set; }
        public required string Message { get; set; }
    }
}