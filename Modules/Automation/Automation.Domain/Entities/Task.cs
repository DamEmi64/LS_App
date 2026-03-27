using Base;

namespace Automation.Domain.Entities
{
    public class Task : Entity
    {
        public int OperationId { get; set; }
        public int Order { get; set; }
        public string Data { get; set; } = string.Empty;
    }
}
