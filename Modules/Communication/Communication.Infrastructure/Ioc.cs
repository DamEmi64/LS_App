using Base;
using Communication.Domain;
using Communication.Domain.Repositories;
using Communication.Infrastructure.Connect.SendEmail.Strategies;
using Communication.Infrastructure.EmailGenerator;
using Communication.Infrastructure.External.Discord;
using Communication.Infrastructure.Repositories;
using Communication.Infrastructure.Services;
using Communication.Infrastructure.Services.SendService;
using CommunicationBase;
using CommunicationBase.Interfaces;
using Discord;
using Discord.WebSocket;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Communication.Infrastructure
{
    public static class IoC
    {
        public static IServiceCollection AddRepos(this IServiceCollection services)
        {
            return services.AddScoped<IEmailRepository, EmailRepository>()
                .AddScoped<ICommunicationHistoryRepository, CommunicationHistoryRepository>()
                .AddScoped<IDiscordRepository, DiscordRepository>()
                .AddScoped<ITemplateRepository, TemplateRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ISendStrategy, SendViaMailjetApiStrategy>()
                .AddScoped<ISendStrategy, SendViaSMTPStrategy>();

            services.Configure<EmailOptions>(AppConfiguration.Get<EmailOptions>());
            return services.AddScoped<ISendService, SendService>()
                .AddScoped<IFluidService, FluidService>()
                .AddFluidParser<EmailFluidParser>(nameof(EmailFluidParser));
        }

        public static IServiceCollection AddDiscord(
                this IServiceCollection services)
        {
            services.AddSingleton<DiscordCommandResolver>();

            services.AddSingleton(sp =>
            {
                var config = new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Debug,
                    GatewayIntents =
                        GatewayIntents.Guilds |
                        GatewayIntents.GuildMessages |
                        GatewayIntents.MessageContent
                };

                return new DiscordSocketClient(config);
            });

            services.AddSingleton<IDiscordCommandDispatcher, DiscordCommandDispatcher>();

            return services;
        }

        public static async Task RegisterDiscordCommands(this IApplicationBuilder app)
        {
            var commandResolver = app.ApplicationServices.GetRequiredService<DiscordCommandResolver>();
            var cmds = commandResolver.DiscoverCommands();
            DiscordOptions options;

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IDiscordRepository>();
                options = scope.ServiceProvider.GetRequiredService<IOptions<DiscordOptions>>().Value;

                await repo.DisableAllCommands();

                foreach (var cmd in cmds)
                {
                    if (repo.Exist(cmd.Key))
                    {
                        await repo.Enable(cmd.Key);
                        continue;
                    }

                    await repo.Add(new Domain.Entities.DiscordCmd
                    {
                        Cmd = cmd.Key,
                        Active = true,
                        Response = cmd.Value.DefaultConfiguration
                    });
                }
            }

            using (var client = new HttpClient() { BaseAddress = new Uri("https://discord.com/api/v10/") })
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", options.Token);
                string url = $"applications/{options.ApplicationId}/commands";

                _ = await client.PutAsync(url, new StringContent("[]", Encoding.UTF8, "application/json"));

                var roots = new Dictionary<string, JObject>();

                foreach (var cmd in cmds)
                {
                    var parts = cmd.Key.Split('/');

                    if (!roots.TryGetValue(parts[0], out var root))
                    {
                        root = new JObject
                        {
                            ["name"] = parts[0],
                            ["description"] = parts[0],
                            ["options"] = new JArray()
                        };

                        roots[parts[0]] = root;
                    }

                    JArray cmdOptions = (JArray)root["options"]!;
                    JObject? current = null;

                    for (int i = 1; i < parts.Length; i++)
                    {
                        var existing = cmdOptions
                            .Children<JObject>()
                            .FirstOrDefault(x => (string?)x["name"] == parts[i]);

                        if (existing == null)
                        {
                            existing = new JObject
                            {
                                ["type"] = 1,
                                ["name"] = parts[i],
                                ["description"] = parts[i],
                                ["options"] = new JArray()
                            };

                            cmdOptions.Add(existing);
                        }

                        current = existing;
                        cmdOptions = (JArray)existing["options"]!;
                    }
                }
                var requestData = roots.Select(x => x.Value);
                using var content = new StringContent(JsonConvert.SerializeObject(requestData.ToList()), Encoding.UTF8, "application/json");
                _ = await client.PutAsync(url, content);
            }
        }
    }
}