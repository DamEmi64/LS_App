using Base;

namespace Events.Domain.Entities
{
    public class EventInvoice : Entity
    {
        public required Event Event { get; set; }
        public Guid InvoiceId { get; set; }
        public required string UserId { get; set; }
        public decimal Value { get; set; }
    }
}
