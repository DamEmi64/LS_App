using Base;

namespace Communication.Domain.Entities
{
    public class CommunicationHistory : Entity
    {
        public required string Subject { get; set; }
        public required string Body { get; set; }
        public required string Recipient { get; set; }
        public required DateTime Date { get; set; }
    }
}
