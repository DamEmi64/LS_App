using Automation.Infrastructure;
using Automation.Infrastructure.Context;
using Automation.Infrastructure.Services.NotifyListener;
using Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Automation.Application
{
    public class AutomationModule : IModule
    {
        public IEnumerable<Operation> Operations => new List<Operation>
        {
        };

        public string Name => "Automation";

        public string Version => "v1.0";

        public IEnumerable<PermissionInfo> Permissions => [PermissionInfo.Create("automation", "Manage automation tasks", false)];

        public IServiceCollection Configure(IServiceCollection services)
        {
            services.AddAutoMapper(opt => opt.AddMaps(typeof(AutomationModule).Assembly));
            services.AddNotifier<NotifyListener>();

            services.AddDatabase<AutomationContext>(AppConfiguration.DefaultConnectionString)
                .AddRepos()
                .AddServices();

            return services;
        }

        public IApplicationBuilder OnStartup(IApplicationBuilder app)
        {
            return app;
        }
    }
}
