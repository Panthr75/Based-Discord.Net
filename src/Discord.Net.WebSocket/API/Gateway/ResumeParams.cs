using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Gateway
{
    internal class ResumeParams
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
        [JsonPropertyName("session_id")]
        public string SessionId { get; set; } = string.Empty;
        [JsonPropertyName("seq")]
        public int Sequence { get; set; }
    }
}
