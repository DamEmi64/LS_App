using Automation.Application;
using Base;
using Communication.Application;
using Drive.API;
using Events.Application;
using Files.Application;
using FilesV2.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RPG.Application;
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
            new DriveModule().Info(),
            new SystemModule().Info(),
            new FilesModule().Info(),
            new RPGModule().Info(),
            new CommunicationModule().Info(),
            new AutomationModule().Info(),
            new EventModule().Info(),
            new FilesV2Module().Info()
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
        }
    }
}