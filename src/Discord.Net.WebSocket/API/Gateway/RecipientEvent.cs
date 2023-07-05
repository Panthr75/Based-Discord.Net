using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Gateway
{
    internal class RecipientEvent
    {
        [JsonPropertyName("user")]
        public User User { get; set; } = null!;
        [JsonPropertyName("channel_id")]
        public ulong ChannelId { get; set; }
    }
}
