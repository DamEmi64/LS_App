/*using Base;
using Connector;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Newtonsoft.Json.Converters;
using Serilog;
using System.Domain.Repositories;
using System.Reflection;


namespace Api
{
    public class Startup
    {
        private IConnectorResolver _connector;
        private readonly IHostApplicationBuilder _builder;

        private const string SystemModuleName = "System";
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
            _connector = builder.InitializeConnector();
            _builder.Services.AddSingleton(_connector);

            if (builder is WebApplicationBuilder webBuilder)
            {
                webBuilder.Host.UseSerilog((context, services, loggerConfig) =>
                {
                    loggerConfig.ReadFrom.Configuration(context.Configuration);
                });
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

        private IServiceCollection AddSwagger(IServiceCollection services, IConfiguration configuration)
        {
            return services.AddEndpointsApiExplorer()
                .AddSwaggerGen(opt =>
                {
                    opt.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "LS API",
                        Version = "v1"
                    });

                    opt.CustomOperationIds(apiDesc =>
                    {
                        var path = apiDesc.RelativePath ?? "endpoint";
                        var method = apiDesc.HttpMethod?.ToLowerInvariant();
                        var controller = apiDesc.ActionDescriptor.RouteValues["controller"];
                        path = path.Split('?')[0];
                        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

                        var parts = new List<string>();

                        foreach (var segment in segments)
                        {
                            if (segment == controller || segment == "api")
                            {
                                continue;
                            }
                            else if (segment.StartsWith("{"))
                            {
                                var nameBy = segment.Trim('{', '}');
                                parts.Add("By" + char.ToUpper(nameBy[0]) + nameBy.Substring(1));
                            }
                            else
                            {
                                parts.Add(char.ToUpper(segment[0]) + segment.Substring(1));
                            }
                        }

                        var name = string.Join("", parts);

                        return method switch
                        {
                            "post" => $"create{name}",
                            "put" => $"update{name}",
                            "get" => $"get{name}",
                            "delete" => $"delete{name}",
                            _ => name
                        };
                    });

                    opt.TagActionsBy(api =>
                    {
                        var controller = api.ActionDescriptor.RouteValues["controller"] ?? "default";

                        return new[]
                        {
                            char.ToLowerInvariant(controller[0]) + controller[1..]
                        };
                    });

                    opt.DocInclusionPredicate((name, api) => true);

                    opt.IgnoreObsoleteActions();
                    opt.IgnoreObsoleteProperties();

                    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
                    if (File.Exists(xmlPath))
                    {
                        opt.IncludeXmlComments(xmlPath);
                    }
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
                catch (ModuleInfoEx.NeccessaryModuleNeededException)
                {
                    Environment.Exit(0);
                }
                catch (ModuleInfoEx.ModuleVersionInvalidException)
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
                UseModules(app);

                app.UseSwagger(o => o.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0);
                app.UseSwaggerUI();

                app.UseAuthentication();
                app.UseAuthorization();
                app.UseMiddleware<ErrorMiddleware>();
                app.UseMiddleware<SerilogMiddleware>();
                app.MapRazorPages();

                app.MapControllerRoute(
                name: "default",
                pattern: "api/{controller=Home}/{action=Index}/{id?}");
                app.MapFallbackToFile("/index.html");
                app.UseSerilogRequestLogging();

                if (AppConfiguration.GetValue(AutoMigrate, true))
                {
                    UpdateDatabases(app);
                }

                UpdateDictionaries(app);

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
                var repository = scope.ServiceProvider.GetRequiredService<IDictionaryRepository>();
                var connector = scope.ServiceProvider.GetRequiredService<IConnectorResolver>();
                app.Logger.LogInformation($"Verify dictionaries...");

                try
                {
                    repository.UpdateDictionaries(dictionaries);
                }
                catch (Exception ex)
                {
                    app.Logger.LogCritical(ex.Message);
                }

                app.Logger.LogInformation($"Dictionaries verified.");

                connector.SetDictionary(dictionaries);
            }
        }
    }
}
*/