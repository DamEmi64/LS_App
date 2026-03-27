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
    }
}