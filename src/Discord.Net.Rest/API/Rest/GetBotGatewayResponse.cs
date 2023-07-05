using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Rest
{
    internal class GetBotGatewayResponse
    {
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
        [JsonPropertyName("shards")]
        public int Shards { get; set; }
        [JsonPropertyName("session_start_limit")]
        public SessionStartLimit SessionStartLimit { get; set; } = null!;
    }
}
