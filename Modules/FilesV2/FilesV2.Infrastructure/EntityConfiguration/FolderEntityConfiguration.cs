using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilesV2.Infrastructure.EntityConfiguration
{
    public class FolderEntityConfiguration : IEntityTypeConfiguration<Domain.Entities.Directory>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Directory> builder)
        {
            builder.HasMany(x => x.Files).WithOne(x => x.Folder).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Children).WithOne(x => x.Parent).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
