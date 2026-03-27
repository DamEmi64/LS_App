namespace Automation.Application.Dtos
{
    public class TaskDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int OperationId { get; set; }
        public int Order { get; set; }
        public object? Data { get; set; }
    }
}
