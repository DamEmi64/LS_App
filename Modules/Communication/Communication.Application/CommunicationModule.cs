using Base;
using Base.Interfaces;
using Communication.Domain;
using Communication.Infrastructure;
using Communication.Infrastructure.Db;
using Communication.Infrastructure.Services.EmailSender;
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
            OperationExtensions.Operation(Domain.Dictionaries.Operations.SendEmail,"Send email","email"),
            OperationExtensions.Operation(Domain.Dictionaries.Operations.GenerateFromTemplate,"Generate email from template","email")
        };

        public string Name => "Communication";

        public string Version => "v0.2";

        public IServiceCollection Configure(IServiceCollection services)
        {
            services.AddAutoMapper(opt => opt.AddMaps(typeof(CommunicationModule).Assembly));

            return services
                .AddDatabase<CommunicationContext>(AppConfiguration.DefaultConnectionString)
                .AddRepos()
                .Configure<EmailOptions>(AppConfiguration.Get<EmailOptions>())
                .AddScoped<IEmailSender, EmailSender>()
                .AddServices();
        }

        public IApplicationBuilder OnStartup(IApplicationBuilder app)
        {
            FluidGenerator.Initialize(app.ApplicationServices.GetServices<IFluidParser>());
            return app;
        }
    }
}