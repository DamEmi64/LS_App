using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPG.Domain.Entities;

namespace RPG.Infrastructure.EntityConfiguration
{
    public class HeroEntityTypeConfiguration : IEntityTypeConfiguration<Hero>
    {
        public void Configure(EntityTypeBuilder<Hero> builder)
        {
            builder.HasOne(x => x.Chapter).WithMany(x => x.Heroes).OnDelete(DeleteBehavior.Cascade);
        }
    }
}