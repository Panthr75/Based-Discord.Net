using System.Text.Json.Serialization;

namespace Discord.API.Gateway
{
    internal class PresenceUpdateParams
    {
        [JsonPropertyName("status")]
        public UserStatus Status { get; set; }
        [JsonPropertyName("since")]
        public long? IdleSince { get; set; }
        [JsonPropertyName("afk")]
        public bool IsAFK { get; set; }
        [JsonPropertyName("activities")]
        public object[]? Activities { get; set; } // TODO, change to interface later
    }
}
