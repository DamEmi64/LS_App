namespace Automation.Domain.Entities
{
    public class SystemAutomat
    {
        public Guid Id { get; set; }
        public required int Key { get; set; }
        public bool Active { get; set; }
        public string? Configuration { get; set; }
    }
}
