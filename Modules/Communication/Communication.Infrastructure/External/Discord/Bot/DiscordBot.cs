using Communication.Domain;
using Communication.Domain.Repositories;
using CommunicationBase.Dtos;
using CommunicationBase.Interfaces;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Communication.Infrastructure.External.Discord
{
    public class DiscordBot : IDiscordBot
    {
        private readonly DiscordSocketClient _client;
        private readonly DiscordOptions _options;
        private readonly IDiscordCommandDispatcher _discordCommandDispatcher;
        private readonly IServiceProvider _services;

        public DiscordBot(
            DiscordSocketClient client,
            IOptions<DiscordOptions> options,
            IDiscordCommandDispatcher discordCommandDispatcher,
            IServiceProvider services)
        {
            _client = client;
            _options = options.Value;
            _discordCommandDispatcher = discordCommandDispatcher;
            _services = services;
        }

        public async Task StartAsync(
            CancellationToken cancellationToken = default)
        {
            await _client.LoginAsync(
                TokenType.Bot,
                _options.Token);

            _client.MessageReceived += OnMessageReceivedAsync;

            await _client.StartAsync();
        }

        private async Task OnMessageReceivedAsync(SocketMessage message)
        {
            if (message.Author.IsBot)
                return;

            if (!message.Content.StartsWith("@LS-API "))
                return;

            var parts = message.Content["@LS-API ".Length..].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var command = parts[0];

            var context = new DiscordCommandContext
            {
                Arguments = parts.Skip(1).ToArray(),
                Username = message.Author.Username,
                UserId = message.Author.Id.ToString(),
                Message = message.Content
            };

            var response = await _discordCommandDispatcher.DispatchAsync(command, context);

            if (response != null)
            {
                if (!string.IsNullOrWhiteSpace(response.Text))
                    await message.Channel.SendMessageAsync(response.Text);

                if (response.File.Length > 0)
                    await message.Channel.SendFileAsync(new MemoryStream(response.File), "file");
            }

            using (var scope = _services.CreateScope())
            {
                var registryRepo = scope.ServiceProvider.GetRequiredService<ICommunicationHistoryRepository>();
                await registryRepo.Add(new Domain.Entities.CommunicationRegistry
                {
                    From = message.Author.Username,
                    To = "Discord Bot",
                    Message = message.Content,
                    Title = $"Discord Command Received {command}",
                });
            }
        }
    }
}
