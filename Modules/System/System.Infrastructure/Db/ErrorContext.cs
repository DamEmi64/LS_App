using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Domain.Entities;
using System.Text;

namespace System.Infrastructure.Db
{
    public class ErrorContext : DbContext
    {
        public ErrorContext(DbContextOptions<ErrorContext> options)
:        base(options)
        {
        }

        public DbSet<Log> Logs { get; set; } = default!;
    }
}
