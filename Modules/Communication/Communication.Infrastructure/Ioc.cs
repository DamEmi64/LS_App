using Base;
using Communication.Domain;
using Communication.Domain.Repositories;
using Communication.Infrastructure.Connect.SendEmail.Strategies;
using Communication.Infrastructure.EmailGenerator;
using Communication.Infrastructure.Repositories;
using Communication.Infrastructure.Services;
using Communication.Infrastructure.Services.SendService;
using CommunicationBase;
using Microsoft.Extensions.DependencyInjection;

namespace Communication.Infrastructure
{
    public static class IoC
    {
        public static IServiceCollection AddRepos(this IServiceCollection services)
        {
            return services.AddScoped<IEmailRepository, EmailRepository>()
                .AddScoped<ICommunicationHistoryRepository,CommunicationHistoryRepository>()
                .AddScoped<ITemplateRepository, TemplateRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ISendStrategy, SendViaMailjetApiStrategy>()
                .AddScoped<ISendStrategy, SendViaSMTPStrategy>();

            services.Configure<EmailOptions>(AppConfiguration.Get<EmailOptions>());
            return services.AddScoped<ISendService, SendService>()
                .AddScoped<IFluidService, FluidService>()
                .AddFluidParser<EmailFluidParser>(nameof(EmailFluidParser));
        }
    }
}