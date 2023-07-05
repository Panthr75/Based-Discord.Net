using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Voice
{
    internal class UdpProtocolInfo
    {
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
        [JsonPropertyName("port")]
        public int Port { get; set; }
        [JsonPropertyName("mode")]
        public string Mode { get; set; } = string.Empty;
    }
}
