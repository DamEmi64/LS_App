using Base;
using Microsoft.AspNetCore.Http;
using RPG.Infrastructure.External.FileConverters;

namespace RPG.Infrastructure.Services
{
    public interface IImportService
    {
        Task<string> ImportFromFile(IFormFile? file, int converterType, string? externalUrl, UserData user);
        Task<byte[]> ExportAsJson(Guid storyId);
    }
}