using Base;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Domain.Entities;
using System.Domain.Repositories;
using System.Infrastructure.Db;
using System.Infrastructure.Filters;
using System.Infrastructure.Hubs;
using System.Infrastructure.JobEngine;
using System.Infrastructure.JobEngine.Milestones;
using System.Infrastructure.Repositories;
using System.Infrastructure.Services.Admin;
using System.Infrastructure.Services.Auth;
using System.Infrastructure.Services.Controller;
using System.Infrastructure.Services.EntityContext;
using System.Infrastructure.Services.Media;
using System.Infrastructure.Services.NotifyService;
using System.Infrastructure.Workers;

namespace System.Infrastructure
{
    public static class IoC
    {
        public static IServiceCollection AddRepos(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddScoped<IProcessRepository, ProcessRepository>()
                .AddScoped<IJobRepository, JobRepository>()
                .AddScoped<IDictionaryRepository, DictionaryRepository>()
                .AddScoped<ILogRepository, LogRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection serviceDescriptors, IConfiguration configuration)
        {
            return serviceDescriptors.AddScoped<IControllerService, ControllerService>()
                .AddScoped<INotifier, Notifier>()
                .AddScoped<IEntityContext, EntityContext>()
                .AddScoped<IMediaProvider, MediaService>();
        }

        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<SystemContext>()
            .AddDefaultTokenProviders();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            services.Configure<IdentityOptions>(options =>
            {
                // NewPassword settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(1);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.SameSite = SameSiteMode.None;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            });

            services.AddCors(options =>
              options.AddDefaultPolicy(
              policy =>
              {
                  policy.AllowAnyHeader();
                  policy.AllowAnyMethod();
                  policy.AllowCredentials()
                        .SetIsOriginAllowed(origin => true);
              }));
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IAdminService, AdminService>();
            services.Configure<AdminPanelOptions>(configuration.GetSection("Admin"));

            services.AddScoped<AdminPanelFilter>();

            return services;
        }

        public static IServiceCollection AddNotifier(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSignalR()
               .AddHubOptions<NotifyHub>(options =>
               {
                   options.EnableDetailedErrors = true;
                   options.MaximumReceiveMessageSize = 1024 * 1024 * 10; // 10 MB
               });

            return services.AddNotifier<HubNotifier>();
        }

        public static IServiceCollection AddDb(this IServiceCollection services, IConfiguration configuration)
            => services.AddDatabase<SystemContext>(configuration.GetConnectionString("DbContext") ?? throw new InvalidOperationException("Connection string 'DbContext' not found."));

        public static IServiceCollection AddErrorDb(this IServiceCollection services, IConfiguration configuration)
            => services.AddDbContext<ErrorContext>(o =>
            {
                o.UseSqlServer(configuration.GetConnectionString("LogContext") ?? AppConfiguration.DefaultConnectionString);
            });

        public static IServiceCollection AddBackgroundService(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = AppConfiguration.DefaultConnectionString;

            return services.AddScoped<IJobEngine, JobEngine.JobEngine>()
                .AddScoped<IMilestoneWorker, MilestoneWorker>()
                .AddScoped<ArchiveLogsWorker>()
                .AddScoped<IJobExecutor, JobExecutor>()
                .AddHangfire(options =>
                {
                    options.UseSqlServerStorage(connectionString);
                    options.UseRecommendedSerializerSettings();
                    options.UseColouredConsoleLogProvider();
                    options.UseDefaultTypeSerializer();
                })
                .AddHangfireServer(options =>
                {
                    options.HeartbeatInterval = TimeSpan.FromMinutes(1);
                    options.StopTimeout = TimeSpan.FromHours(12);
                    options.Queues = GetQueues(configuration).ToArray();
                    options.WorkerCount = 5;
                });
        }

        private static IEnumerable<string> GetQueues(IConfiguration configuration)
        {
            yield return "default";

            foreach (var operation in GetOperations())
            {
                yield return operation.Queue;
            }
        }

        private static List<Operation> GetOperations()
        {
            var list = new List<Operation>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var item in assemblies)
            {
                try
                {
                    var moduleType = item.GetTypes().FirstOrDefault(x => !x.IsInterface && !x.IsAbstract && x.GetInterfaces().Contains(typeof(IModule)));

                    if (moduleType is null || moduleType.IsInterface || moduleType.IsAbstract)
                    {
                        continue;
                    }

                    var module = Activator.CreateInstance(moduleType) as IModule;

                    if (module is null)
                    {
                        continue;
                    }

                    list.AddRange(module.Operations);
                }
                catch (Exception) { }
            }

            return list;
        }
    }
}