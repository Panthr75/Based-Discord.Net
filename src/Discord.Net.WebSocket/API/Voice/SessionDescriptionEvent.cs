using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Voice
{
    internal class SessionDescriptionEvent
    {
        [JsonPropertyName("secret_key")]
        public byte[] SecretKey { get; set; } = Array.Empty<byte>();
        [JsonPropertyName("mode")]
        public string Mode { get; set; } = string.Empty;
    }
}
