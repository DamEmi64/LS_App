using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi;
using Serilog;
using System.Reflection;

namespace Base
{
    public abstract class ConnectorStartup : BaseStartup, IConnector
    {
        private const string AutoMigrate = "autoMigrate";
        private const string Banner = @"
###############################
#       API IS RUNNING...     #
#           version {0}       #
###############################
";

        public ConnectorStartup(IHostApplicationBuilder builder) : base(builder)
        {
        }

        public abstract string Version { get; }

        public override IReadOnlyCollection<PermissionInfo> Permissions => Modules.SelectMany(x => x.Module.Permissions).ToList();

        public override void OnConfigure(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureModules(services);
            ConfigureLogging(services);
            ConfigureSwagger(services);
            OnConnectorConfigure(services);
        }

        protected void ConfigureModules(IServiceCollection services)
        {
            var pageBuilder = services.AddRazorPages();
            var controllerBuilder = services.AddControllers();

            foreach (var module in Modules.Select(x => x.Module))
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
        }

        protected void ConfigureLogging(IServiceCollection services)
        {
            if (_builder is WebApplicationBuilder webBuilder)
            {
                webBuilder.Host.UseSerilog((context, services, loggerConfig) =>
                {
                    loggerConfig.ReadFrom.Configuration(context.Configuration);
                });
            }
        }

        protected void ConfigureSwagger(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer()
                .AddSwaggerGen(opt =>
                {
                    opt.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "LS API",
                        Version = "v1"
                    });

                    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Description = "JWT Authorization header using the Bearer scheme.",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT"
                    });
                    opt.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", document, null)] = new List<string>()
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

        public abstract void OnConnectorStartup(WebApplication app);
        public abstract void OnConnectorConfigure(IServiceCollection services);

        public override void OnStartup(WebApplication app)
        {
            UseModules(app);

            app.UseSwagger(o => o.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0);
            app.UseSwaggerUI();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages();

            OnConnectorStartup(app);

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

            ProvideBasicRoles(app);

            app.Logger.LogInformation(Banner, AppConfiguration.Version);
        }

        private void UseModules(WebApplication application)
        {
            try
            {
                foreach (var module in Modules.Select(x => x.Module))
                {
                    module.OnStartup(application);
                }
            }
            catch (Exception)
            {
            }
        }

        private void ProvideBasicRoles(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var connectClient = scope.ServiceProvider.GetRequiredService<IConnect>();
                connectClient.Send(new ProvideBasicRoles(Permissions.ToList())).Wait();

            }
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
                var repository = scope.ServiceProvider.GetRequiredService<IDictionaryProvider>();
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
            }
        }
    }
}
