using Automation.Infrastructure.Services;
using Automation.Infrastructure.Services.AutomationService;
using Base.Automation;
using Microsoft.Extensions.DependencyInjection;

namespace Automation.Infrastructure
{
    public static class IoC
    {
        public static IServiceCollection AddRepos(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddScoped<Domain.Repositories.IAutomatRepository, Repositories.AutomatRepository>()
                .AddScoped<Domain.Repositories.ITaskRepository, Repositories.TaskRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddScoped<IAutomationService, AutomationService>()
                .AddScoped<IAutomationResolver, AutomationResolver>();
        }
    }
}
