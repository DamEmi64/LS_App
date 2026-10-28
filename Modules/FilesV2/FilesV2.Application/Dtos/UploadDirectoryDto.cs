using Microsoft.AspNetCore.Http;
namespace FilesV2.Application.Dtos
{
    public class UploadDirectoryDto : CreateDirectoryDto
    {
       public required IFormFile Zip { get; set; }
    }
}
