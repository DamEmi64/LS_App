using Newtonsoft.Json;

namespace Communication.Application.Dtos
{
    public class WebhookDto
    {
        [JsonProperty("event")]
        public string? Event { get; set; }

        [JsonProperty("CustomID")]
        public string? CustomId { get; set; }
    }
}
