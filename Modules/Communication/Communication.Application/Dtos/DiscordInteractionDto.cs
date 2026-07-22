using System;
using System.Collections.Generic;
using System.Text;

namespace Communication.Application.Dtos
{
    public class DiscordInteractionDto
    {
        public int Type { get; set; }

        public DiscordApplicationCommandDataDto? Data { get; set; }

        public string Id { get; set; } = string.Empty;

        public string ApplicationId { get; set; } = string.Empty;

        public DiscordUserDto? Member { get; set; }
    }
}
