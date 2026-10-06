using Invoices.Domain.Entities;

namespace Invoices.Domain.Repositories
{
    public interface IInvoiceRepository
    {
        Task<List<Invoice>> GetByCollector(string userId);
        Task<List<Invoice>> GetByRecipient(string userId);
        Task<Invoice?> GetWithDetails(Guid id);
        Task Add(Invoice invoice);
        Task Update(Invoice invoice);
        Task ReplacePositions(Invoice invoice, List<InvoicePosition> positions);
        Task Remove(Invoice invoice);
        Task<bool> MarkPositionPaid(Invoice invoice, Guid positionId, int paidStatus);
    }
}
