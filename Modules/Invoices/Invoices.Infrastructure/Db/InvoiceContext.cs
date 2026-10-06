using Base;
using Invoices.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Invoices.Infrastructure.Db
{
    public class InvoiceContext : DbContextBase<InvoiceContext>
    {
        public InvoiceContext(DbContextOptions<InvoiceContext> options, IEntityContext entityContext) 
            : base(options, entityContext)
        {
        }

        public DbSet<Invoice> Invoices { get; set; } = default!;
        public DbSet<InvoicePosition> InvoicePositions { get; set; } = default!;
        public DbSet<UserData> InvoiceUsers { get; set; } = default!;
        public DbSet<InvoiceDocument> InvoiceDocuments { get; set; } = default!;

        public override string ContextName => "Invoices";
    }
}
