using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Rest
{
    internal class ModifyStageInstanceParams
    {
        [JsonPropertyName("topic")]
        public Optional<string> Topic { get; set; }

        [JsonPropertyName("privacy_level")]
        public Optional<StagePrivacyLevel> PrivacyLevel { get; set; }
    }
}
