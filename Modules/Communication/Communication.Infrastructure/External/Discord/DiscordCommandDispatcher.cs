using Communication.Domain.Repositories;
using CommunicationBase.Dtos;
using CommunicationBase.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Communication.Infrastructure.External.Discord
{
    public sealed class DiscordCommandDispatcher : IDiscordCommandDispatcher
    {
        private readonly IServiceProvider _services;
        private readonly DiscordCommandResolver _resolver;

        public DiscordCommandDispatcher(
            IServiceProvider services,
            DiscordCommandResolver resolver)
        {
            _services = services;
            _resolver = resolver;
        }

        public async Task<DiscordResponse?> DispatchAsync(
            string command,
            DiscordCommandContext context,
            CancellationToken cancellationToken = default)
        {
            var descriptor = _resolver.Resolve(command);

            if (descriptor == null)
                return null;

            using (var scope = _services.CreateScope())
            {
                var _discordRepository = scope.ServiceProvider.GetRequiredService<IDiscordRepository>();
                var discordCmd = await _discordRepository.GetCmd(
                command);

                if (discordCmd is null || !discordCmd.Active)
                    return new DiscordResponse { Text = "This command is disabled." };
                    
                context.Configuration = discordCmd.Response;

                var instance = ActivatorUtilities.CreateInstance(
                    scope.ServiceProvider,
                    descriptor.CommandClass);

                var task = (Task<DiscordResponse>)descriptor.Method.Invoke(instance, [context])!;

                return await task;
            }
        }
    }
}