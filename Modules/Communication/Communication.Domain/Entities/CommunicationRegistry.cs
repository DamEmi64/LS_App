using Base;

namespace Communication.Domain.Entities
{
    public class CommunicationRegistry : Entity
    {
        public required string Title { get; set; }
        public required string Message { get; set; }
        public required string From { get; set;  }
        public required string To { get; set; }
        public string? CorrelationId { get; set; }
    }
}
