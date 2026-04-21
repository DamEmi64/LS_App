using Base;
using Communication.Domain;
using Communication.Infrastructure.EmailGenerator;
using Communication.Infrastructure.Repositories;
using Communication.Infrastructure.Services;
using Communication.Infrastructure.Services.SendService;
using CommunicationBase.Interfaces;
using Files.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Communication.Infrastructure
{
    public static class IoC
    {
        public static IServiceCollection AddRepos(this IServiceCollection services)
        {
            return services.AddScoped<IEmailRepository, EmailRepository>()
                .AddScoped<ITemplateRepository, TemplateRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.Configure<EmailOptions>(AppConfiguration.Get<EmailOptions>());
            var emailFluidParser = new EmailFluidParser();
            return services.AddScoped<ISendService, SendService>()
                .AddScoped<IFluidService, FluidService>()
                .AddScoped<IFluidParser,EmailFluidParser>(s => emailFluidParser)
                .AddKeyedScoped<IFluidParser, EmailFluidParser>(nameof(EmailFluidParser), (s, o) => emailFluidParser);
        }
    }
}