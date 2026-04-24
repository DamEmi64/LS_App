using Base;
using Microsoft.EntityFrameworkCore;

namespace Translations
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext(DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        public DbSet<DictionaryItem> Dictionaries { get; set; } = default!;
    }
}
