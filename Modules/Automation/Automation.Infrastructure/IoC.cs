using Automation.Domain.Repositories;
using Automation.Infrastructure.Repositories;
using Automation.Infrastructure.Services;
using Automation.Infrastructure.Services.AutomationService;
using AutomationBase;
using Base.Automation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Automation.Infrastructure
{
    public static class IoC
    {
        public static IServiceCollection AddRepos(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddScoped<Domain.Repositories.IAutomatRepository, Repositories.AutomatRepository>()
                .AddScoped<ISystemAutomateRepository, SystemAutomateRepository>()
                .AddScoped<Domain.Repositories.ITaskRepository, Repositories.TaskRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddScoped<IAutomationService, AutomationService>()
                .AddScoped<IAutomationResolver, AutomationResolver>();
        }

        public static async Task RegisterSystemAutomates(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var automates = scope.ServiceProvider.GetServices<ISystemAutomate>();
                var repo = scope.ServiceProvider.GetRequiredService<ISystemAutomateRepository>();

                foreach (var automate in automates)
                {
                    var dbAutomat = await repo.Get(automate.Key);

                    if (dbAutomat is null)
                    {
                        await repo.Add(new Domain.Entities.SystemAutomat
                        {
                            Key = automate.Key,
                            Id = Guid.NewGuid(),
                            Active = true,
                            Configuration = JsonConvert.SerializeObject(automate.Configuration)
                        });
                    }
                }
                
                var automatsToDelete = repo.GetAll().Where(a => !automates.Any(aa => aa.Key == a.Key)).ToList();
                foreach (var automat in automatsToDelete)
                {
                    await repo.Remove(automat.Key);
                }

                await repo.SaveChanges();
            }
        }
    }
}
