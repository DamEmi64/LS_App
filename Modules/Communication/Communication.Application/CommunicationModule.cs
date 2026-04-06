using Base;
using Base.Interfaces;
using Communication.Domain;
using Communication.Infrastructure;
using Communication.Infrastructure.Db;
using Communication.Infrastructure.Services.EmailSender;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Communication.Application
{
    public class CommunicationModule : IModule
    {
        public IEnumerable<Operation> Operations => new List<Operation>
        {
            OperationExtensions.Operation(Domain.Dictionaries.Operations.SendEmail,"Send email","send_email"),
            OperationExtensions.Operation(Domain.Dictionaries.Operations.GenerateFromTemplate,"Generate email from template","gen_email")
        };

        public string Name => "Communication";

        public string Version => "v0.2";

        public IServiceCollection Configure(IServiceCollection services)
        {
            return services
                .AddDatabase<CommunicationContext>(AppConfiguration.DefaultConnectionString)
                .AddRepos()
                .Configure<EmailOptions>(AppConfiguration.Get<EmailOptions>())
                .AddScoped<IEmailSender, EmailSender>()
                .AddServices();
        }

        public IApplicationBuilder OnStartup(IApplicationBuilder app)
        {
            return app;
        }
    }
}