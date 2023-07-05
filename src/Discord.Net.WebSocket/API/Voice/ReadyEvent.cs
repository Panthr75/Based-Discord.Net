using System.Text.Json;
using System;
using System.Text.Json.Serialization;

namespace Discord.API.Voice
{
    internal class ReadyEvent
    {
        [JsonPropertyName("ssrc")]
        public uint SSRC { get; set; }
        [JsonPropertyName("ip")]
        public string Ip { get; set; } = string.Empty;
        [JsonPropertyName("port")]
        public ushort Port { get; set; }
        [JsonPropertyName("modes")]
        public string[] Modes { get; set; } = Array.Empty<string>();
        [JsonPropertyName("heartbeat_interval")]
        [Obsolete("This field is erroneous and should not be used", true)]
        public int HeartbeatInterval { get; set; }
    }
}
