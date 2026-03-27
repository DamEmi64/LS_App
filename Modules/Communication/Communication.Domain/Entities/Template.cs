using Base;

namespace Communication.Domain.Entities
{
    public class Template : Entity
    {
        public required string Subject { get; set; }
        public required string Body { get; set; }
    }
}