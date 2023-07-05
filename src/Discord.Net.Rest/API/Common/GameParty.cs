using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class GameParty
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("size")]
        public long[] Size { get; set; } = Array.Empty<long>();
    }
}
