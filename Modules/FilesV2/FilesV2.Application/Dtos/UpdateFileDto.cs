using Microsoft.AspNetCore.Http;

namespace FilesV2.Application.Dtos
{
    public class UpdateFileDto
    {
        public UpdateFileDto()
        {
        }

        public IFormFile? File { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Guid? DirectoryId { get; set; }
        public bool? Public { get; set; }
    }
}
