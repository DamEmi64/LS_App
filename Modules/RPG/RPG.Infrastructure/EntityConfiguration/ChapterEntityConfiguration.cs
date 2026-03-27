using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPG.Domain.Entities;

namespace RPG.Infrastructure.EntityConfiguration
{
    public class ChapterEntityConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.HasOne(x => x.Story).WithMany(x => x.Chapters).OnDelete(DeleteBehavior.Cascade);
        }
    }
}