using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Voice
{
    internal class SelectProtocolParams
    {
        [JsonPropertyName("protocol")]
        public string Protocol { get; set; } = string.Empty;
        [JsonPropertyName("data")]
        public UdpProtocolInfo Data { get; set; } = null!;
    }
}
