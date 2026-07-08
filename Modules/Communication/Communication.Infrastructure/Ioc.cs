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
                    GatewayIntents =
                        GatewayIntents.Guilds |
                        GatewayIntents.GuildMessages |
                        GatewayIntents.MessageContent
                };

                return new DiscordSocketClient(config);
            });

            services.AddSingleton<IDiscordCommandDispatcher, DiscordCommandDispatcher>();
            services.AddSingleton<IDiscordBot, DiscordBot>();

            return services;
        }

        public static void InitializeDiscordBot(this IApplicationBuilder app)
        {
            var commandResolver = app.ApplicationServices.GetRequiredService<DiscordCommandResolver>();
            var cmds = commandResolver.DiscoverCommands();

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IDiscordRepository>();

                repo.DisableAllCommands();

                foreach (var cmd in cmds)
                {
                    if (repo.Exist(cmd.Key))
                    {
                        repo.Enable(cmd.Key);
                        continue;
                    }

                    repo.Add(new Domain.Entities.DiscordCmd
                    {
                        Cmd = cmd.Key,
                        Active = true,
                        Configuration = cmd.Value.DefaultConfiguration
                    });
                }
            }


            var bot = app.ApplicationServices.GetRequiredService<IDiscordBot>();
            bot.StartAsync();
        }
    }
}