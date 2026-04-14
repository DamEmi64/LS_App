using Microsoft.AspNetCore.Http;
using RPG.Infrastructure.External.FileConverters;

namespace RPG.Application.Dtos
{
    public class ImportDto
    {
        public required IFormFile File { get; set; }
        public int ConverterType { get; set; }
        public string? ExternalUrl { get; set; }
    }
}
