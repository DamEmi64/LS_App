using Base;
using Drive.API.External.Cloudflare;
using Drive.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Drive.API
{
    public class DriveModule : IModule
    {
        public IEnumerable<Operation> Operations => [];

        public string Name => "Drive";

        public string Version => "v1.0";

        public IEnumerable<PermissionInfo> Permissions => [];

        public IServiceCollection Configure(IServiceCollection services)
        {
            services.AddScoped<CloudflareClient>();
            services.AddMediaProvider<CloudflareMediaProvider>("cloudflare");
            return services;
        }

        public IApplicationBuilder OnStartup(IApplicationBuilder app)
        {
            return app;
        }
    }
}
