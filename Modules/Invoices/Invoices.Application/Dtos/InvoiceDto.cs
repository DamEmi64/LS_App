namespace Invoices.Application.Dtos
{
    public class InvoiceDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string CollectorId { get; set; }
        public required string CollectorLogin { get; set; }
        public required string RecipientId { get; set; }
        public required string RecipientLogin { get; set; }
        public List<InvoicePositionDto> Positions { get; set; } = [];
    }

    public class InvoicePositionDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public decimal Value { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int Status { get; set; }
    }
}
