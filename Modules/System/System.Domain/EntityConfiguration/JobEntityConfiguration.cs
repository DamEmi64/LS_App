using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;

namespace System.Domain.EntityConfiguration
{
    public class JobEntityConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.HasOne(x => x.Parent).WithMany(x => x.Children).OnDelete(DeleteBehavior.NoAction);
        }
    }
}