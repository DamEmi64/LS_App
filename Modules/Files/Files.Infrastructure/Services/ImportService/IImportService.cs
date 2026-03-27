using Base;

namespace Files.Infrastructure.Services.DownloadService
{
    public interface IImportService
    {
        Task<string> ImportFile(Domain.Entities.File file, UserData userData);
    }
}