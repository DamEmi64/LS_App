namespace Events.Application.Dtos
{
    public class CreateEventInvoiceDto
    {
        public Guid EventId { get; set; }
        public required string Title { get; set; }
        public List<EventInvoicePositionDto> Positions { get; set; } = [];
    }

    public class EventInvoiceAllocationDto
    {
        public required string UserId { get; set; }
        public decimal Value { get; set; }
    }

    public class EventInvoicePositionDto
    {
        public required string PositionTitle { get; set; }
        public List<EventInvoiceAllocationDto> Allocations { get; set; } = [];
    }

    public class EventInvoiceDto
    {
        public Guid EventId { get; set; }
        public string EventTitle { get; set; } = string.Empty;
        public DateTime? EventDate { get; set; }
        public Guid InvoiceId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string? UserLogin { get; set; }
        public decimal Value { get; set; }
    }
}
