using Discord.Net.Converters;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class AuditLogChange
    {
        [JsonPropertyName("key")]
        public string ChangedProperty { get; set; } = string.Empty;

        [JsonPropertyName("new_value")]
        [JsonConverter(typeof(JsonNodeConverter))]
        public JsonNode? NewValue { get; set; }

        [JsonPropertyName("old_value")]
        [JsonConverter(typeof(JsonNodeConverter))]
        public JsonNode? OldValue { get; set; }
    }
}
