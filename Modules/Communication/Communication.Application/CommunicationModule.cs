using Base;
using Communication.Domain;
using Communication.Infrastructure;
using Communication.Infrastructure.Db;
using CommunicationBase;
using CommunicationBase.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Communication.Application
{
    public class CommunicationModule : IModule
    {
        public IEnumerable<Operation> Operations => new List<Operation>
        {
            Extensions.Operation(Domain.Dictionaries.Operations.SendEmail,"Send email","email"),
            Extensions.Operation(Domain.Dictionaries.Operations.GenerateFromTemplate,"Generate email from template","email")
        };

        public string Name => "Communication";

        public string Version => "v0.2";

        public IEnumerable<PermissionInfo> Permissions => [PermissionInfo.Create("communication", "Manage and send Emails", true)];

        public IServiceCollection Configure(IServiceCollection services)
        {
            services.AddAutoMapper(opt => opt.AddMaps(typeof(CommunicationModule).Assembly));

            return services
                .AddDatabase<CommunicationContext>(AppConfiguration.DefaultConnectionString)
                .AddRepos()
                .Configure<EmailOptions>(AppConfiguration.Get<EmailOptions>())
                .AddServices();
        }

        public IApplicationBuilder OnStartup(IApplicationBuilder app)
        {
            FluidGenerator.Initialize(app.ApplicationServices.GetServices<IFluidParser>());
            return app;
        }
    }
}