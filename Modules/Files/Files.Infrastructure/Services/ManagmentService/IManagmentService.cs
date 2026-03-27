using Base;

namespace Files.Infrastructure.Services.ManagmentService
{
    public interface IManagmentService
    {
        Task<string> CopyFile(Domain.Entities.File file, string destination, UserData userData);

        Task<string> DeleteFile(Domain.Entities.File file, UserData userData);

        Task<string> MoveFile(Domain.Entities.File file, string destination, UserData userData);
    }
}