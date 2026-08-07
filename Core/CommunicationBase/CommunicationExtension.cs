using Base;
using CommunicationBase.Dtos;
using CommunicationBase.Events;
using CommunicationBase.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

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

        public static Task<FluentResults.Result> SendEmailAsync(this IConnect connect, string to, string subject, string body, string? from = null, string? correlationId = null)
        {
            var cmd = new SendEmail(to, subject, body, from, correlationId);

            return connect.Send(cmd);
        }

        public static Task<FluentResults.Result<DiscordResponse>> ExecuteDiscordCmdAsync(this IConnect connect, string cmd, DiscordCommandContext commandContext)
        {
            var cmdEvent = new ExecuteDiscordCmdEvent(cmd, commandContext);

            return connect.Send<ExecuteDiscordCmdEvent, DiscordResponse>(cmdEvent);
        }

        public static DiscordCommandContext ToCommandContext(this JsonElement root)
        {
            // The invoking user lives under "member.user" in guilds, or "user" directly in DMs.
            JsonElement userElement;
            if (root.TryGetProperty("member", out var member) && member.TryGetProperty("user", out var memberUser))
            {
                userElement = memberUser;
            }
            else if (root.TryGetProperty("user", out var dmUser))
            {
                userElement = dmUser;
            }
            else
            {
                throw new InvalidOperationException("Interaction payload contains neither 'member.user' nor 'user'.");
            }

            string userId = userElement.GetProperty("id").GetString() ?? string.Empty;
            string username = userElement.GetProperty("username").GetString() ?? string.Empty;

            var data = root.GetProperty("data");
            string commandName = data.GetProperty("name").GetString() ?? string.Empty;

            string[] arguments = ExtractArguments(data);

            // Reconstruct a text-style message, e.g. "/report daily verbose"
            string message = arguments.Length > 0
                ? $"/{commandName} {string.Join(' ', arguments)}"
                : $"/{commandName}";

            return new DiscordCommandContext
            {
                UserId = userId,
                Username = username,
                Message = message,
                Arguments = arguments
            };
        }

        private static string[] ExtractArguments(JsonElement data)
        {
            if (!data.TryGetProperty("options", out var options) || options.ValueKind != JsonValueKind.Array)
            {
                return [];
            }

            var result = new List<string>();
            foreach (var option in options.EnumerateArray())
            {
                if (!option.TryGetProperty("value", out var value))
                {
                    continue;
                }

                string stringValue = value.ValueKind switch
                {
                    JsonValueKind.String => value.GetString() ?? string.Empty,
                    JsonValueKind.Number => value.GetRawText(),
                    JsonValueKind.True => "true",
                    JsonValueKind.False => "false",
                    _ => value.GetRawText()
                };

                result.Add(stringValue);
            }

            return result.ToArray();
        }
    }
}
