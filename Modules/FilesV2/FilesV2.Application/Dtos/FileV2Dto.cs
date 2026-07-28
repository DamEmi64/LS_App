namespace FilesV2.Application.Dtos
{
    public class FileV2Dto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Owner { get; set; } = string.Empty;
        public bool Public { get; set; }
        public Guid? DirectoryId { get; set; }
        public string Path { get; set; } = string.Empty;
        public List<FileUserDto> FileUsers { get; set; } = new();
    }
}
