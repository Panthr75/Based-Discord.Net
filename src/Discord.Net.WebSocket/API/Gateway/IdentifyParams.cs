using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord.API.Gateway
{
    internal class IdentifyParams
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
        [JsonPropertyName("properties")]
        public IDictionary<string, string> Properties { get; set; } = null!;
        [JsonPropertyName("large_threshold")]
        public int LargeThreshold { get; set; }
        [JsonPropertyName("shard")]
        public Optional<int[]> ShardingParams { get; set; }
        [JsonPropertyName("presence")]
        public Optional<PresenceUpdateParams> Presence { get; set; }
        [JsonPropertyName("intents")]
        public Optional<int> Intents { get; set; }
    }
}
