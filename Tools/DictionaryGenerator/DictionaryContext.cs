using Base.Entities;
using Microsoft.EntityFrameworkCore;

public class DictionaryContext : DbContext
{
    public DictionaryContext(DbContextOptions<DictionaryContext> options)
        : base(options)
    {
    }

    public DbSet<DictionaryItem> Dictionaries { get; set; } = default!;
}

