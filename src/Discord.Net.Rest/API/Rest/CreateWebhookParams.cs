using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Rest
{
    internal class CreateWebhookParams
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("avatar")]
        public Optional<Image?> Avatar { get; set; }
    }
}
