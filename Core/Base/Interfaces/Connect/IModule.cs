using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Base
{
    /// <summary>
    ///     Module interface
    /// </summary>
    public interface IModule
    {
        /// <summary>
        ///     Module configuration 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        IServiceCollection Configure(IServiceCollection services);


        /// <summary>
        ///     Module configuration on startup
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        IApplicationBuilder OnStartup(IApplicationBuilder app);


        /// <summary>
        ///     Module operations
        /// </summary>
        IEnumerable<Operation> Operations { get; }

        /// <summary>
        ///     Module name
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Module version
        /// </summary>
        string Version { get; }
    }
}