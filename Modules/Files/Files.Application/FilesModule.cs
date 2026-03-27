using Base;
using Files.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Infrastructure.Db;

namespace Files.Application
{
    public class FilesModule : IModule
    {
        public IEnumerable<Operation> Operations => new List<Operation>
        {
            OperationExtensions.Operation(Domain.Dictionaries.Operations.ImportFile,"Import file","download_file"),
            OperationExtensions.Operation(Domain.Dictionaries.Operations.MoveFile,"Move file","move_file"),
            OperationExtensions.Operation(Domain.Dictionaries.Operations.CopyFile,"Copy file","copy_file"),
            OperationExtensions.Operation(Domain.Dictionaries.Operations.DeleteFile,"Delete file","delete_file"),
        };

        public string Name => "Files";

        public string Version => "v0.2";

        public IServiceCollection Configure(IServiceCollection services)
        {
            services.AddAutoMapper(opt => opt.AddMaps(typeof(FilesModule).Assembly));

            return services
                .AddDatabase<FilesContext>(AppConfiguration.DefaultConnectionString)
                .AddRepos()
                .AddHttpClient()
                .AddServices();
        }

        public IApplicationBuilder OnStartup(IApplicationBuilder app)
        {
            return app;
        }
    }
}