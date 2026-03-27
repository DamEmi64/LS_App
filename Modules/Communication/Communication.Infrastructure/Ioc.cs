using Base;
using Communication.Domain;
using Communication.Infrastructure.Repositories;
using Communication.Infrastructure.Services.SendService;
using Files.Domain.Repositories;
using Microsoft.Extensions.Configuration;
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

            return services.AddScoped<ISendService, SendService>();
        }
    }
}