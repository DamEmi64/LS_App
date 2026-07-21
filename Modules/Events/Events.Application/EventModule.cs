using Base;
using Events.Infrastructure;
using Events.Infrastructure.Db;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Application
{
    public class EventModule : IModule
    {
        public IEnumerable<Operation> Operations => new[]
        {
            Extensions.Operation(Domain.Dictionaries.Operations.SendInvitation,"Send invitation","event"),
            Extensions.Operation(Domain.Dictionaries.Operations.SendReminder,"Send reminder","event"),
        };

        public string Name => "Events";

        public string Version => "v1.0";

        public IEnumerable<PermissionInfo> Permissions => [PermissionInfo.Create("events", "Manage events", true),];

        public IServiceCollection Configure(IServiceCollection services)
        {
            services.AddAutoMapper(opt => opt.AddMaps(typeof(EventModule).Assembly));
            return services.AddDatabase<EventContext>(AppConfiguration.DefaultConnectionString)
                .AddServices()
                .AddRepositories();
        }

        public IApplicationBuilder OnStartup(IApplicationBuilder app)
        {
            return app;
        }
    }
}
