using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class Relationship
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }
        [JsonPropertyName("user")]
        public User User { get; set; } = null!;
        [JsonPropertyName("type")]
        public RelationshipType Type { get; set; }
    }
}
