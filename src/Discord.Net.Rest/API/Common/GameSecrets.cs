using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class GameSecrets
    {
        [JsonPropertyName("match")]
        public string Match { get; set; } = string.Empty;
        [JsonPropertyName("join")]
        public string Join { get; set; } = string.Empty;
        [JsonPropertyName("spectate")]
        public string Spectate { get; set; } = string.Empty;
    }
}
