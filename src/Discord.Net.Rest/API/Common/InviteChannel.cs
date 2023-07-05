using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class InviteChannel
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public int Type { get; set; }
    }
}
