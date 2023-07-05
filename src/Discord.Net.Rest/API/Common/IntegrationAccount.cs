using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class IntegrationAccount
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}
