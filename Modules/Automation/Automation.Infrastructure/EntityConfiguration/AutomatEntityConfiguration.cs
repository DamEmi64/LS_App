using Automation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RPG.Infrastructure.EntityConfiguration
{
    public class AutomatEntityConfiguration : IEntityTypeConfiguration<Automat>
    {
        public void Configure(EntityTypeBuilder<Automat> builder)
        {
            builder.HasMany(x => x.Tasks).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Triggers).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}