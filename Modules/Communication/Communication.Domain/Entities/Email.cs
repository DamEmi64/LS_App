using Base;

namespace Communication.Domain.Entities
{
    public class Email : Entity
    {
        public required string Subject { get; set; }
        public required string Body { get; set; }
        public required string Sender { get; set; }
        public required string Recipient { get; set; }
        public DateTimeOffset? SentDate { get; set; }
        public int Status { get; set; }
        public string? ExternalId { get; set; }
    }
}