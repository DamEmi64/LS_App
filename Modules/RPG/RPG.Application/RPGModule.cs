using Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RPG.Infrastructure;
using RPG.Infrastructure.External.FileConverters;
using RPG.Infrastructure.External.Firebase;
using RPG.Infrastructure.Hubs;
using RPG.Infrastructure.Jobs;
using System.Infrastructure.Db;

namespace RPG.Application
{
    public class RPGModule : IModule
    {
        public IEnumerable<Operation> Operations => new List<Operation>
        {
            OperationExtensions.Operation(Domain.Dictionaries.Operations.GenerateSummary,"Generate summary","gen_summary"),
            OperationExtensions.Operation(Domain.Dictionaries.Operations.SentToFirebase,"Send to firebase","firebase"),
            OperationExtensions.Operation(Domain.Dictionaries.Operations.GenerateStoryFromSummary,"Generate story from summary","gen_summary"),
            OperationExtensions.Operation(Domain.Dictionaries.Operations.GetLastRPG,"Get last RPG","gen_summary"),
            OperationExtensions.Operation(Domain.Dictionaries.Operations.ImportRPGFromFile,"Import RPG from file","import_rpg")

        };

        public string Name => "RPG sessions";

        public string Version => "v0.8";

        public IServiceCollection Configure(IServiceCollection services)
        {
            services.Configure<FirebaseOptions>(AppConfiguration.Get<FirebaseOptions>());

            services.AddAutoMapper(opt => opt.AddMaps(typeof(RPGModule).Assembly));
            services.AddAutoMapper(opt => opt.AddMaps(typeof(FileDataMapper).Assembly));

            return services
                .AddDatabase<RPGContext>(AppConfiguration.DefaultConnectionString)
                .AddRepos()
                .AddServices();
        }

        public IApplicationBuilder OnStartup(IApplicationBuilder app)
        {
            if (app is WebApplication webApplication)
            {
                webApplication.MapHub<RPGHub>("/rpghub");
            }

            return app;
        }
    }
}