using Microsoft.Extensions.DependencyInjection;

namespace AutomationBase
{
    public static class Extensions
    {
        public static IServiceCollection AddSystemAutomat<T>(this IServiceCollection services) where T: class, ISystemAutomate
        {
           return services.AddScoped<ISystemAutomate, T>(); 
        } 
    }
}
