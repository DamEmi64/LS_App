using Base;
using CommunicationBase.Dtos;
using CommunicationBase.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CommunicationBase
{
    public static class CommunicationExtension
    {
        public static T GetProperty<T>(this FluidContext context, string name)
        {
            if (context.Model.TryGetValue(name, out var value))
            {
                if (value is T typedValue)
                {
                    return typedValue;
                }
            }
            throw new KeyNotFoundException($"Property '{name}' not found in context.");
        }

        public static void SetProperty<T>(this FluidContext context, string key, T value)
        {
            ArgumentNullException.ThrowIfNull(value);

            if (context.Model.ContainsKey(key))
            {
                context.Model[key] = value;
            }
            else
            {
                context.Model.Add(key, value);
            }
        }

        public static IServiceCollection AddFluidParser<T>(this IServiceCollection services, string? key = null) where T : FluidParserModel
        {
            services.AddScoped<T>();
            services.AddScoped<IFluidParser, T>();

            if (!string.IsNullOrEmpty(key))
            {
                services.AddKeyedScoped<IFluidParser>(key, (sp, _) => sp.GetRequiredService<T>());
            }

            return services;
        }

        public static Task<FluentResults.Result> SendEmailAsync(this IConnect connect, string to,string subject,string body,string? from = null)
        {
            var cmd = new SendEmail(to,subject,body,from);

            return connect.Send(cmd);
        } 
    }
}
