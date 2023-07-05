using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class IntegrationApplication
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("icon")]
        public Optional<string> Icon { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("bot")]
        public Optional<User> Bot { get; set; }
    }
}
