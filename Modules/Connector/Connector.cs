using Automation.Application;
using Base;
using Communication.Application;
using Events.Application;
using Files.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RPG.Application;
using SharedEvents.Auth;
using System.Application;
using System.Infrastructure.Services.ConnectorResolver;

namespace Connector
{
    public class Connector : ConnectorStartup
    {
        public Connector(IHostApplicationBuilder builder) : base(builder)
        {
        }

        public override IReadOnlyCollection<ModuleInfo> Modules => new List<ModuleInfo>
        {
            new SystemModule().Info(),
            new FilesModule().Info(),
            new RPGModule().Info(),
            new CommunicationModule().Info(),
            new AutomationModule().Info(),
            new EventModule().Info(),
        };

        public override string Version => AppConfiguration.Version;

        public override void OnConnectorConfigure(IServiceCollection services)
        {
            services.AddScoped<IConnectorService, ConnectorService>(provider => new ConnectorService(this));
        }

        public override void OnConnectorStartup(WebApplication app)
        {
            app.UseMiddleware<SerilogMiddleware>();
            app.UseMiddleware<ErrorMiddleware>();
            ProvideBasicRoles(app);
            
        }

        public void ProvideBasicRoles(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var connectClient = scope.ServiceProvider.GetRequiredService<IConnect>();
                connectClient.Send(new ProvideBasicRoles(Permissions.ToList())).Wait();

            }
        }
    }
}