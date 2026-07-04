using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;

namespace Base;

public abstract class BaseStartup
{
    protected readonly IHostApplicationBuilder _builder;

    protected BaseStartup(
        IHostApplicationBuilder builder)
    {
        _builder = builder;

        ConfigureServices(builder.Services, builder.Configuration);
    }

    protected void ConfigureServices(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.Converters.Add(
                    new StringEnumConverter()));

        services.AddScoped<IConnect, ConnectClient>();
        ConfigureMediator(services);
        AppConfiguration.Initialize(configuration, Permissions, Modules);
        OnConfigure(services, configuration);
    }

    public abstract void OnConfigure(IServiceCollection services, IConfiguration configuration);
    public abstract void OnStartup(WebApplication app);

    public virtual IReadOnlyCollection<PermissionInfo> Permissions => new List<PermissionInfo>();
    public virtual IReadOnlyCollection<ModuleInfo> Modules => new List<ModuleInfo>();

    public WebApplication Build()
    {
        if (_builder is not WebApplicationBuilder webBuilder)
            throw new InvalidOperationException();

        var app = webBuilder.Build();
        OnStartup(app);

        return app;
    }

    private void ConfigureMediator(IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(GetAssemblies());
        });
    }

    private IEnumerable<Assembly> GetAssemblies()
    {
        var loaded = new HashSet<Assembly>();
        var queue = new Queue<Assembly>(
            Modules.Select(m => m.Module.GetType().Assembly));

        while (queue.Count > 0)
        {
            var assembly = queue.Dequeue();

            if (!loaded.Add(assembly))
                continue;

            foreach (var reference in assembly.GetReferencedAssemblies())
            {
                queue.Enqueue(Assembly.Load(reference));
            }
        }

        return loaded.ToArray();
    }
}