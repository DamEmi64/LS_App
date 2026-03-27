using Automation.Infrastructure;
using Automation.Infrastructure.Context;
using Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Automation.Application
{
    public class AutomationModule : IModule
    {
        public IEnumerable<Operation> Operations => new List<Operation>
        {
        };

        public string Name => "Automation module";

        public string Version => "v0.2 Alpha";

        public IServiceCollection Configure(IServiceCollection services)
        {
            services.AddAutoMapper(opt => opt.AddMaps(typeof(AutomationModule).Assembly));

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
