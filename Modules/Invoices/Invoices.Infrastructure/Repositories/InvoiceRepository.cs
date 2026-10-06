using Invoices.Domain.Entities;
using Invoices.Domain.Repositories;
using Invoices.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Invoices.Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly InvoiceContext _context;

        public InvoiceRepository(InvoiceContext context) => _context = context;

        public Task<List<Invoice>> GetByCollector(string userId) => WithDetails()
            .AsNoTracking().Where(x => x.Collector.UserId == userId).ToListAsync();

        public Task<List<Invoice>> GetByRecipient(string userId) => WithDetails()
            .AsNoTracking().Where(x => x.Recipient.UserId == userId).ToListAsync();

        public Task<Invoice?> GetWithDetails(Guid id) => WithDetails().FirstOrDefaultAsync(x => x.Id == id);

        public async Task Add(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Invoice invoice)
        {
            await _context.SaveChangesAsync();
        }

        public async Task ReplacePositions(Invoice invoice, List<InvoicePosition> positions)
        {
            _context.InvoicePositions.RemoveRange(invoice.Positions);
            invoice.Positions = positions;
            await _context.SaveChangesAsync();
        }

        public async Task Remove(Invoice invoice)
        {
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> MarkPositionPaid(Invoice invoice, Guid positionId, int paidStatus)
        {
            var position = invoice.Positions.FirstOrDefault(x => x.Id == positionId);
            if (position is null) return false;
            position.Status = paidStatus;
            await _context.SaveChangesAsync();
            return true;
        }

        private IQueryable<Invoice> WithDetails() => _context.Invoices
            .Include(x => x.Positions).Include(x => x.Collector).Include(x => x.Recipient);
    }
}
