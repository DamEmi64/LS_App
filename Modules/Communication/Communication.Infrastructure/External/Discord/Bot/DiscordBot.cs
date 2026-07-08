using Communication.Domain;
using CommunicationBase.Dtos;
using CommunicationBase.Interfaces;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;

namespace Communication.Infrastructure.External.Discord
{
    public class DiscordBot : IDiscordBot
    {
        private readonly DiscordSocketClient _client;
        private readonly DiscordOptions _options;
        private readonly IDiscordCommandDispatcher _discordCommandDispatcher;

        public DiscordBot(
            DiscordSocketClient client,
            IOptions<DiscordOptions> options,
            IDiscordCommandDispatcher discordCommandDispatcher)
        {
            _client = client;
            _options = options.Value;
            _discordCommandDispatcher = discordCommandDispatcher;
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

            if (!message.Content.StartsWith("/"))
                return;

            var parts = message.Content[1..].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var command = parts[0];

            var context = new DiscordCommandContext
            {
                Arguments = parts.Skip(1).ToArray(),
                Username = message.Author.Username,
                UserId = message.Author.Id.ToString(),
                Message = message.Content
            };

            var response = await _discordCommandDispatcher.DispatchAsync(command, context);

            if (!string.IsNullOrWhiteSpace(response))
                await message.Channel.SendMessageAsync(response);
        }
    }
}
