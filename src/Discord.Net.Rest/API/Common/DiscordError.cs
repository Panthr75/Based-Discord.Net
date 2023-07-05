using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class DiscordError
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
        [JsonPropertyName("code")]
        public DiscordErrorCode Code { get; set; }
        [JsonPropertyName("errors")]
        public Optional<ErrorDetails[]> Errors { get; set; }
    }
}
