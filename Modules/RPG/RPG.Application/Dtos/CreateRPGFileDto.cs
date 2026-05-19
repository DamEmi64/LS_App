using Microsoft.AspNetCore.Http;
namespace RPG.Application.Dtos
{
    public class CreateRPGFileDto
    {
        public Guid? FileId { get; set; }

        public required IFormFile File { get; set; }
    }
}
