namespace Invoices.Application.Dtos
{
    public class SaveInvoiceDto
    {
        public required string Title { get; set; }
        public required string RecipientId { get; set; }
        public List<SaveInvoicePositionDto> Positions { get; set; } = [];
    }

    public class SaveInvoicePositionDto
    {
        public string? Title { get; set; }
        public decimal Value { get; set; }
    }
}
