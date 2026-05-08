using Base;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Infrastructure;
using System.Infrastructure.Hubs;
using System.Infrastructure.JobEngine.Milestones;
using System.Infrastructure.Services.Admin;
using System.Infrastructure.Workers;

namespace System.Application
{
    public class SystemModule : IModule
    {
        public IEnumerable<Operation> Operations => [Extensions.Operation(0, "default", "default")];

        public string Name => "System";

        public string Version => "v1.0";

        public IServiceCollection Configure(IServiceCollection services)
        {
            services.AddAutoMapper(opt => opt.AddMaps(typeof(SystemModule).Assembly));

            services.AddLogging();

            return services
                .AddDb(AppConfiguration.Root)
                .AddErrorDb(AppConfiguration.Root)
                .AddRepos(AppConfiguration.Root)
                .AddBackgroundService(AppConfiguration.Root)
                .AddServices(AppConfiguration.Root)
                .AddNotifier(AppConfiguration.Root)
                .AddAuth(AppConfiguration.Root);
        }

        public IApplicationBuilder OnStartup(IApplicationBuilder app)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHangfireDashboard(options: new DashboardOptions
            {
                AppPath = "/admin"
            });

            app.UseMiddleware<EntityContextMiddleware>();

            RecurringJob.AddOrUpdate<MilestoneWorker>("milestone-manager", job => job.Execute(), "*/15 * * * *");
            RecurringJob.AddOrUpdate<ArchiveLogsWorker>("archive-logs-cleaner", job => job.Execute(DateTime.Now.Date), Cron.Weekly());

            if (app is WebApplication webApplication)
            {
                webApplication.MapHub<NotifyHub>("/notify");
                ProvideBasicRoles(webApplication);

            }

            return app;
        }

        public void ProvideBasicRoles(WebApplication app)
        {
            try
            {
                using (var scope = app.Services.CreateScope())
                {
                    var adminService = scope.ServiceProvider.GetRequiredService<IAdminService>();
                    var connector = scope.ServiceProvider.GetRequiredService<IConnectorResolver>();
                    var roles = adminService.GetRoles();
                    if (!roles.Select(x => x.Name).Contains("admin"))
                    {
                        adminService.CreateRole("admin", connector.Permissions.Select(x => x.Key)).Wait();
                    }
                    else
                    {
                        adminService.UpdateRole("admin", connector.Permissions.Select(x => x.Key)).Wait();
                    }

                    if (!roles.Select(x => x.Name).Contains("user"))
                    {
                        adminService.CreateRole("user", connector.Permissions.Where(x => x.IsBasic).Select(x => x.Key)).Wait();
                    }
                    else
                    {
                        adminService.UpdateRole("user", connector.Permissions.Where(x => x.IsBasic).Select(x => x.Key)).Wait();
                    }
                }
            }
            catch
            {

            }
        }
    }
}