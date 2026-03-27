using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPG.Domain.Entities;

namespace RPG.Infrastructure.EntityConfiguration
{
    public class PlaceEntityTypeConfiguration : IEntityTypeConfiguration<Place>
    {
        public void Configure(EntityTypeBuilder<Place> builder)
        {
            builder.HasOne(x => x.Chapter).WithMany(x => x.Places).OnDelete(DeleteBehavior.Cascade);
        }
    }
}