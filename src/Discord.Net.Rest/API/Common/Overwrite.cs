using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class Overwrite
    {
        [JsonPropertyName("id")]
        public ulong TargetId { get; set; }
        [JsonPropertyName("type")]
        public PermissionTarget TargetType { get; set; }
        [JsonPropertyName("deny")]
        public string Deny { get; set; } = string.Empty;
        [JsonPropertyName("allow")]
        public string Allow { get; set; } = string.Empty;
    }
}
