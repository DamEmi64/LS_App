using Api.Setup;
using Base;
using Base.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.OpenApi;
using Newtonsoft.Json.Converters;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Infrastructure.Db;

namespace Api
{
    public static class IoC
    {
        private const string Config = "config";
        private const string AutoMigrate = "autoMigrate";

        private const string Banner = @"
###############################
#       API IS RUNNING...     #
#           version {0}       #
###############################
";

        public static void AddLogger(this WebApplicationBuilder builder)
        {
            var columnsOptions = new Serilog.Sinks.MSSqlServer.ColumnOptions();
            columnsOptions.Store.Remove(StandardColumn.Properties);
            columnsOptions.Store.Remove(StandardColumn.MessageTemplate);

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .WriteTo.Console()
            .WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("LogContext") ?? AppConfiguration.DefaultConnectionString,
                sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
                {
                    TableName = "Logs",
                    AutoCreateSqlTable = true,
                    AutoCreateSqlDatabase = true
                },
                columnOptions: columnsOptions)
            .CreateLogger();

            builder.Host.UseSerilog();
        }

        public static void Configure(IServiceCollection services, ConfigurationManager configuration)
        {
            AppConfiguration.Initialize(configuration);
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
            options.SerializerSettings.Converters.Add(new StringEnumConverter())); ;

            var connector = GetConnector(configuration);

            services.AddModules(configuration, connector)
               .AddSingleton<IConnector>(connector)
               .AddSwagger(configuration);
            services.AddTransient<ErrorMiddleware>();
            services.AddSwaggerGenNewtonsoftSupport();
        }

        public static Connector.Connector GetConnector(IConfiguration configuration)
        {
            var baseUrl = configuration.GetValue("urls", string.Empty);
            var version = configuration.GetValue("version", string.Empty);
            return new Connector.Connector(baseUrl ?? string.Empty, version);
        }

        private static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddEndpointsApiExplorer()
                .AddSwaggerGen(opt =>
                {
                    opt.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "My API",
                        Version = "v1"  // This must be a valid version string like "v1"
                    });
                    opt.IgnoreObsoleteActions();
                });
        }

        private static IServiceCollection AddModules(this IServiceCollection services, ConfigurationManager configuration, IConnector connector)
        {
            var pageBuilder = services.AddRazorPages();
            var controllerBuilder = services.AddControllers();

            foreach (var module in connector.Modules.Select(x => x.Module))
            {
                try
                {
                    var assembly = module.GetType().Assembly;

                    try
                    {
                        pageBuilder.AddApplicationPart(assembly);
                    }
                    catch (Exception) { }

                    module.Configure(services);
                    controllerBuilder.AddApplicationPart(assembly);
                }
                catch (NeccessaryModuleNeededException)
                {
                    Environment.Exit(0);
                }
                catch (ModuleVersionInvalidException)
                {
                    Environment.Exit(0);
                }
                catch (Exception) { }
            }

            return services;
        }

        private static void UseModules(this WebApplication application)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            try
            {
                foreach (var assembly in assemblies)
                {
                    var moduleType = assembly.GetTypes().FirstOrDefault(x => !x.IsInterface && !x.IsAbstract && x.GetInterfaces().Contains(typeof(IModule)));

                    if (moduleType is null || moduleType.IsInterface || moduleType.IsAbstract)
                    {
                        continue;
                    }

                    var module = Activator.CreateInstance(moduleType) as IModule;

                    if (module is null)
                    {
                        continue;
                    }

                    module.OnStartup(application);
                }
            }
            catch (Exception)
            {
            }
        }

        public static IServiceCollection AddDatabase<T>(this IServiceCollection services, string connString) where T : DbContext, IDbContextBase
        {
            return services.AddDbContext<T>(o =>
            {
                o.UseSqlServer(connString);
                o.ReplaceService<IHistoryRepository, MigrationHistoryRepository>();
            });
        }

        public static void Setup(this WebApplication app, IConfiguration configuration)
        {
            app.UseSwagger(o => o.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0);
            app.UseSwaggerUI();
            app.UseMiddleware<ErrorMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapFallbackToFile("/index.html");

            if (configuration.GetSection(Config).GetValue<bool>(AutoMigrate, true))
            {
                app.UpdateDatabases();
            }

            app.UpdateDictionaries();
            app.UseModules();
            app.Logger.LogInformation(Banner, AppConfiguration.Version);
        }

        private static List<Type> GetContextType(Microsoft.Extensions.Logging.ILogger logger)
        {
            List<Type> contextTypes = new();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                contextTypes.AddRange(assembly.GetTypes().Where(x => !x.IsAbstract && x.GetInterface(nameof(IDbContextBase)) == typeof(IDbContextBase)));
            }

            return contextTypes;
        }

        public static void UpdateDatabases(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                app.Logger.LogInformation("Searching for contexts...");
                foreach (var type in GetContextType(app.Logger))
                {
                    app.Logger.LogInformation($"{type.Name} found.Updating...");
                    try
                    {
                        var item = scope.ServiceProvider.GetService(type);

                        if (item is DbContext context)
                        {
                            context.Database.Migrate();
                        }

                        app.Logger.LogInformation($"Migrations for {type.Name} applied.");
                    }
                    catch (Exception ex)
                    {
                        app.Logger.LogError(ex, null);
                    }
                }
            }
        }

        public static void UpdateDictionaries(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dictionaries = EntityDictionary.GetDictionaries();
                var context = scope.ServiceProvider.GetRequiredService<SystemContext>();
                var conector = scope.ServiceProvider.GetRequiredService<IConnector>();
                app.Logger.LogInformation($"Verify dictionaries...");

                try
                {
                    foreach (var item in dictionaries)
                    {
                        try
                        {
                            if (!context.Dictionaries.Any(x => x.Key == item.Key))
                            {
                                context.Dictionaries.Add(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            app.Logger.LogCritical(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    app.Logger.LogCritical(ex.Message);
                }

                app.Logger.LogInformation($"Dictionaries verified.");

                context.SaveChanges();

                if (conector is Connector.Connector connectorInstance)
                {
                    connectorInstance.DictionaryItems = context.Dictionaries;
                }
            }
        }
    }
}