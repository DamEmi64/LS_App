using Base;
using CommunicationBase.Attributes;
using CommunicationBase.Dtos;
using CommunicationBase.Interfaces;
using Newtonsoft.Json;

namespace Communication.Infrastructure.External.Discord.Commands
{
    public class FreeStuffCommands : IDiscordCommandsWrapper
    {
        private readonly string _freeGamesUrl =
            "https://www.gamerpower.com/api/giveaways";

        [DiscordCommand("free-giveaway", "Should generate list of free games (default config : No needed)")]
        public async Task<DiscordResponse> FreeGames(DiscordCommandContext ctx)
        {
            using var httpClient = new HttpClient();

            try
            {
                var response = await httpClient.GetAsync(_freeGamesUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                var allGiveaways =
                    JsonConvert.DeserializeObject<List<GamerPowerGame>>(json)
                    ?? new List<GamerPowerGame>();

                var games = allGiveaways
                    .Where(x =>
                        !string.IsNullOrWhiteSpace(x.Type) &&
                        x.Type.Equals("Game", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (games.Count == 0)
                {
                    return new DiscordResponse
                    {
                        Text = "No free games found."
                    };
                }

                var gameTexts = games
                    .Select(x => x.ToString())
                    .ToList();

                return new DiscordResponse
                {
                    Text = TemplateFormatter.Format(
                        "List of free games is:\n{games:joinByLine}",
                        new
                        {
                            games = gameTexts
                        })
                };
            }
            catch (HttpRequestException ex)
            {
                return new DiscordResponse
                {
                    Text = $"Failed to retrieve free games from GamerPower: {ex.Message}"
                };
            }
            catch (JsonException ex)
            {
                return new DiscordResponse
                {
                    Text = $"Failed to parse GamerPower response: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new DiscordResponse
                {
                    Text = $"Failed to retrieve free games: {ex.Message}"
                };
            }
        }

        public class GamerPowerGame
        {
            public string? Title { get; set; }
            public string? Platforms { get; set; }

            [JsonProperty("open_giveaway")]
            public string? OpenGiveawayUrl { get; set; }
            public string? Type { get; set; }

            public override string ToString()
            {
                return $"{Title} - ({Platforms})\nUrl: {OpenGiveawayUrl}";
            }
        }
    }
}
