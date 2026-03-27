using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;

namespace System.Domain.EntityConfiguration
{
    public class ProcessEntityConfiguration : IEntityTypeConfiguration<Process>
    {
        public void Configure(EntityTypeBuilder<Process> builder)
        {
            builder.HasMany(x => x.Jobs).WithOne(x => x.Process).OnDelete(DeleteBehavior.Cascade);
        }
    }
}