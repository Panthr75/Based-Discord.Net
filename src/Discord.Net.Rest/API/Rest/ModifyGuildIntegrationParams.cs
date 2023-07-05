using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Rest
{
    internal class ModifyGuildIntegrationParams
    {
        [JsonPropertyName("expire_behavior")]
        public Optional<int> ExpireBehavior { get; set; }
        [JsonPropertyName("expire_grace_period")]
        public Optional<int> ExpireGracePeriod { get; set; }
        [JsonPropertyName("enable_emoticons")]
        public Optional<bool> EnableEmoticons { get; set; }
    }
}
