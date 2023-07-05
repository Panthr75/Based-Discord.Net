using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Rest
{
    internal class CreateStageInstanceParams
    {
        [JsonPropertyName("channel_id")]
        public ulong ChannelId { get; set; }

        [JsonPropertyName("topic")]
        public string Topic { get; set; } = string.Empty;

        [JsonPropertyName("privacy_level")]
        public Optional<StagePrivacyLevel> PrivacyLevel { get; set; }
    }
}
