using FilesV2.Domain.Repositories;
using FilesV2.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FilesV2.Infrastructure
{
    public static class IoC
    {
        public static IServiceCollection AddRepos(this IServiceCollection services)
        {
            return services
                .AddScoped<IFileRepository,FileRepository>()
                .AddScoped<IFolderRepository, FolderRepository>();
        }
    }
}
