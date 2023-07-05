using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class SocketFrame
    {
        [JsonPropertyName("op")]
        public int Operation { get; set; }
        [JsonPropertyName("t")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Type { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Sequence { get; set; }
        [JsonPropertyName("d")]
        public object? Payload { get; set; } = null;
    }
}
