using Base;

namespace FilesV2.Domain.Entities
{
    public class Directory : Entity
    {
        public required string Title { get; set; }

        public Directory? Parent { get; set; }

        public List<Directory> Children { get; set; } = new();

        public List<File> Files { get; set; } = new();
    }
}
