using Base;

namespace FilesV2.Domain.Entities
{
    public class File : Entity
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required FileUser Owner { get; set; }
        public bool Public { get; set; } = false;
        public Guid Content { get; set; }
        public List<FileUser> Users { get; set; } = new();

        public Directory? Folder { get; set; }

        public string Path => GetFilePath();

        private string GetFilePath()
        {
            var catalog = Folder;
            var pathItems = new List<string>();

            while (catalog is not null)
            {
                pathItems.Add(catalog.Title);
                catalog = catalog.Parent;
            }

            pathItems.Reverse();
            pathItems.Add(Title);
            return string.Join("/", pathItems);
        }

    }
}
