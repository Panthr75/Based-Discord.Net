using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class Ban
    {
        [JsonPropertyName("user")]
        public User User { get; set; } = null!;
        [JsonPropertyName("reason")]
        public string? Reason { get; set; }
    }
}
