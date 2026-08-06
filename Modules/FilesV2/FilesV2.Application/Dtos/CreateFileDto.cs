using Microsoft.AspNetCore.Http;

namespace FilesV2.Application.Dtos
{
    public class CreateFileDto
    {
        public required IFormFile File { get; set; }
        public string? Description { get; set; }
        public required string Title { get; set; }
        public Guid? DirectoryId { get; set; }
        public bool Public { get; set; } = false;
    }
}
