using Files.Domain.Repositories;
using Files.Infrastructure.Repositories;
using Files.Infrastructure.Services.DownloadService;
using Files.Infrastructure.Services.ManagmentService;
using Microsoft.Extensions.DependencyInjection;

namespace Files.Infrastructure
{
    public static class IoC
    {
        public static IServiceCollection AddRepos(this IServiceCollection services)
        {
            return services.AddScoped<IFileRepository, FileRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddScoped<IImportService, ImportService>()
                .AddScoped<IManagmentService, ManagmentService>();
        }
    }
}