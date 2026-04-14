using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPG.Infrastructure.External.FileConverters.Json
{
    public class OldJsonStory
    {
        [JsonProperty("RPGName")]
        public string? Title { get; set; }

        [JsonProperty("ImageData")]
        public string? Image { get; set; }

        [JsonProperty("Heroes")]
        public List<OldJsonHero>? Heroes { get; set; }

        [JsonProperty("Places")]
        public List<OldJsonElement>? Places { get; set; }

        [JsonProperty("Npcs")]
        public List<OldJsonElement>? Npcs { get; set; }

        [JsonProperty("Stories")]
        public List<OldJsonElement>? Chapters { get; set; }
    }

    public class OldJsonElement
    {
        [JsonProperty("Name")]
        public string? Title { get; set; }

        [JsonProperty("Description")]
        public string? Description { get; set; }

        [JsonProperty("ImageData")]
        public string? Image { get; set; }
    }

    public class OldJsonHero :OldJsonElement
    {
        [JsonProperty("Player")]
        public string? Player { get; set; }

        [JsonProperty("Equipment")]
        public string? Equipment { get; set; }

        [JsonProperty("Skills")]
        public List<OldJsonSkill>? Skills { get; set; }
    }

    public class  OldJsonSkill
    {
        public string? Name { get; set; }

        public int Value { get; set; }
    }
}
