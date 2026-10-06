using Invoices.Domain.Entities;

namespace Invoices.Domain.Repositories
{
    public interface IInvoiceRepository
    {
        Task<List<Invoice>> GetByCollector(string userId);
        Task<List<Invoice>> GetByRecipient(string userId);
        Task<Invoice?> GetWithDetails(Guid id);
        Task<InvoiceDocument?> GetDocument(Guid invoiceId);
        Task SaveDocument(InvoiceDocument document);
        Task Add(Invoice invoice);
        Task Update(Invoice invoice);
        Task Remove(Invoice invoice);
        Task<bool> MarkPositionPaid(Invoice invoice, Guid positionId, int paidStatus);
        Task RemovePosition(InvoicePosition position);
    }
}
