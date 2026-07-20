using Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RPG.Infrastructure;
using RPG.Infrastructure.External.FileConverters;
using RPG.Infrastructure.External.Firebase;
using RPG.Infrastructure.Hubs;
using System.Infrastructure.Db;

namespace RPG.Application
{
    public class RPGModule : IModule
    {
        public IEnumerable<Operation> Operations => new List<Operation>
        {
            Extensions.Operation(Domain.Dictionaries.Operations.GenerateSummary,"Generate summary","gen_summary"),
            Extensions.Operation(Domain.Dictionaries.Operations.SentToFirebase,"Send to firebase","external"),
            Extensions.Operation(Domain.Dictionaries.Operations.GenerateStoryFromSummary,"Generate story from summary","gen_summary"),
            Extensions.Operation(Domain.Dictionaries.Operations.GetLastRPG,"Get last RPG","gen_summary"),
            Extensions.Operation(Domain.Dictionaries.Operations.ImportRPGFromFile,"Import RPG from file","external")

        };

        public string Name => "RPG sessions";

        public string Version => "v1.0";

        public IEnumerable<PermissionInfo> Permissions => [
            PermissionInfo.Create("rpg","Read RPG sessions",true),
            PermissionInfo.Create("rpg-write","Manage RPG sessions",false),
            PermissionInfo.Create("rpg-draft","Manage drafts of RPG sessions",false)];

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