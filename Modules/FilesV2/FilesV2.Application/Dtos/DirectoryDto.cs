namespace FilesV2.Application.Dtos
{
    public class DirectoryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }
        public int ChildDirectoryCount { get; set; }
        public int FileCount { get; set; }
        public string Owner { get; set; } = string.Empty;
        public bool Public { get; set; }
        public List<FileUserDto> FileUsers { get; set; } = new();
    }
}
