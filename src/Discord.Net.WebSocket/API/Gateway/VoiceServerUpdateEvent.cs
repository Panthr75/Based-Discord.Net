using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Gateway
{
    internal class VoiceServerUpdateEvent
    {
        [JsonPropertyName("guild_id")]
        public ulong GuildId { get; set; }
        [JsonPropertyName("endpoint")]
        public string Endpoint { get; set; } = string.Empty;
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
    }
}
