using Base;
using Base.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Newtonsoft.Json.Converters;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Infrastructure.Db;

namespace Api
{
    public class Startup
    {
        private Connector.Connector _connector;
        private readonly IHostApplicationBuilder _builder;

        private const string AutoMigrate = "autoMigrate";
        private const string Banner = @"
###############################
#       API IS RUNNING...     #
#           version {0}       #
###############################
";

        public Startup(IHostApplicationBuilder builder)
        {
            _builder = builder;
            AppConfiguration.Initialize(builder.Configuration);
            _connector = GetConnector(builder.Configuration);
            builder.Services.AddSingleton<IConnector>(_connector);

            //LOGS ARE SAVED BY HOSTING SERVICE
/*            if (builder.Environment.IsDevelopment())
            {
                Log.Logger = new LoggerConfiguration()
                               .MinimumLevel.Information()
                               .Enrich.FromLogContext()
                               .WriteTo.Console() // optional but recommended
                               .WriteTo.MSSqlServer(
                                   connectionString: AppConfiguration.GetConnectionString("LogContext") ?? AppConfiguration.DefaultConnectionString,
                                   sinkOptions: new MSSqlServerSinkOptions
                                   {
                                       TableName = "Logs",
                                       AutoCreateSqlTable = true,
                                       AutoCreateSqlDatabase = true
                                   })
                               .CreateLogger();
            }
            else
            {
                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .Enrich.FromLogContext()
                .WriteTo.Console() // optional but recommended
                .WriteTo.MSSqlServer(
                    connectionString: AppConfiguration.GetConnectionString("LogContext") ?? AppConfiguration.DefaultConnectionString,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "Logs",
                        AutoCreateSqlTable = true,
                        AutoCreateSqlDatabase = true
                    })
                .CreateLogger();
            }*/

            if (builder is WebApplicationBuilder webBuilder)
            {
                webBuilder.Host.UseSerilog();
            }

            Configure(builder.Services, builder.Configuration);
        }

        public void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));

            AddModules(services, configuration);
            AddSwagger(services, configuration)
                .AddSwaggerGenNewtonsoftSupport();

            services.AddTransient<ErrorMiddleware>();
        }

        private Connector.Connector GetConnector(IConfiguration configuration)
        {
            var baseUrl = configuration.GetValue("urls", string.Empty);
            var version = configuration.GetValue("version", string.Empty);
            return new Connector.Connector(baseUrl ?? string.Empty, version);
        }

        private IServiceCollection AddSwagger(IServiceCollection services, IConfiguration configuration)
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

        private IServiceCollection AddModules(IServiceCollection services, IConfiguration configuration)
        {
            var pageBuilder = services.AddRazorPages();
            var controllerBuilder = services.AddControllers();

            foreach (var module in _connector.Modules.Select(x => x.Module))
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

        private void UseModules(WebApplication application)
        {
            try
            {
                foreach (var module in _connector.Modules.Select(x => x.Module))
                {
                    module.OnStartup(application);
                }
            }
            catch (Exception)
            {
            }
        }

        public WebApplication Build()
        {
            if (_builder is WebApplicationBuilder webApplicationBuilder)
            {
                var app = webApplicationBuilder.Build();

                app.UseSwagger(o => o.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0);
                app.UseSwaggerUI();
                app.UseMiddleware<ErrorMiddleware>();

                app.UseAuthentication();
                app.UseAuthorization();
                app.MapRazorPages();

                app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
                app.MapFallbackToFile("/index.html");

                if (AppConfiguration.GetValue(AutoMigrate, true))
                {
                    UpdateDatabases(app);
                }

                UpdateDictionaries(app);

                UseModules(app);
                app.Logger.LogInformation(Banner, AppConfiguration.Version);

                return app;
            }

            throw new InvalidOperationException($"{_builder} is not web application builder");
        }

        private List<Type> GetContextType()
        {
            List<Type> contextTypes = new();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                contextTypes.AddRange(assembly.GetTypes().Where(x => !x.IsAbstract && x.GetInterface(nameof(IDbContextBase)) == typeof(IDbContextBase)));
            }

            return contextTypes;
        }

        public void UpdateDatabases(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                app.Logger.LogInformation("Searching for contexts...");
                foreach (var type in GetContextType())
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

        private void UpdateDictionaries(WebApplication app)
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
