using System.Text.Json.Serialization;
using System;
using Discord.Net.Converters;

namespace Discord.API
{
    internal class GameTimestamps
    {
        [JsonPropertyName("start")]
        [JsonConverter(typeof(UnixTimestampConverter))]
        public Optional<DateTimeOffset> Start { get; set; }
        [JsonPropertyName("end")]
        [JsonConverter(typeof(UnixTimestampConverter))]
        public Optional<DateTimeOffset> End { get; set; }
    }
}
