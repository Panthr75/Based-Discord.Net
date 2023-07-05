using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Gateway
{
    internal class HelloEvent
    {
        [JsonPropertyName("heartbeat_interval")]
        public int HeartbeatInterval { get; set; }
    }
}
