using Microsoft.EntityFrameworkCore;
using System.Domain.Entities;

namespace System.Infrastructure.Db
{
    public class ErrorContext : DbContext
    {
        public ErrorContext(DbContextOptions<ErrorContext> options)
: base(options)
        {
        }

        public DbSet<Log> Logs { get; set; } = default!;
    }
}
