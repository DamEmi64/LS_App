using Base;
using Microsoft.EntityFrameworkCore;

namespace Translations
{
    public class DictionaryContext : DbContext
    {
        public DictionaryContext(DbContextOptions<DictionaryContext> options)
            : base(options)
        {
        }

        public DbSet<DictionaryItem> Dictionaries { get; set; } = default!;
    }
}
