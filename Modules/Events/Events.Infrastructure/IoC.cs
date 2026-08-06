using Base;
using Base.Automation;
using Events.Domain;
using Events.Domain.Repositories;
using Events.Infrastructure.Repositories;
using Events.Infrastructure.Services.AutomationResolver;
using Events.Infrastructure.Services.InvitationService;
using Events.Infrastructure.Services.ReminderService;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Infrastructure
{
    public static class IoC
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.Configure<EventOptions>(AppConfiguration.Get<EventOptions>());
            return services
                .AddScoped<IReminderService, ReminderService>()
                .AddScoped<IInvitationService, InvitationService>()
                .AddScoped<IAutomationResolver, EventAutomationResolver>();
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services.AddScoped<IEventRepository, EventRepository>();
        }
    }
}
