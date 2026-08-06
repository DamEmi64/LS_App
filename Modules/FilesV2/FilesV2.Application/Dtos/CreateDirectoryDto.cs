namespace FilesV2.Application.Dtos
{
    public class CreateDirectoryDto
    {
        public required string Title { get; set; }
        public Guid? ParentId { get; set; }
    }
}
