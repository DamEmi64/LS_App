using Microsoft.AspNetCore.Http;

namespace RPG.Application.Dtos
{
    public class ImportDto
    {
        public IFormFile? File { get; set; }
        public int ConverterType { get; set; }
        public string? ExternalUrl { get; set; }
    }
}
