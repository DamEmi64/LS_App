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

            using (var client = new HttpClient() { BaseAddress = new Uri("https://discord.com/api/v10/")})
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", options.Token);
                string url = $"applications/{options.ApplicationId}/commands";

                foreach (var cmd in cmds)
                {
                    var json = string.Empty;

                    if (cmd.Value.Arguments.Count > 0)
                    {
                        var cmdOptions = cmd.Value.Arguments.Select(x => new
                        {
                            name = x.Key,
                            description = x.Key,
                            type = 3,
                            required = x.Value
                        });

                        json = JsonConvert.SerializeObject(new
                        {
                            name = cmd.Key,
                            description = cmd.Value.Command,
                            type = 1,
                            options = cmdOptions
                        });
                    }
                    else
                    {
                        json = JsonConvert.SerializeObject(new
                        {
                            name = cmd.Key,
                            description = cmd.Value.Command,
                            type = 1
                        });
                    }

                    using var content = new StringContent(json, Encoding.UTF8, "application/json");
                    _ = await client.PostAsync(url, content);
                }
            }
        }
    }
}