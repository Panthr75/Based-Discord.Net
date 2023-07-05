using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class EmbedField
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("value")]
        public string Value { get; set; } = string.Empty;
        [JsonPropertyName("inline")]
        public bool Inline { get; set; }
    }
}
