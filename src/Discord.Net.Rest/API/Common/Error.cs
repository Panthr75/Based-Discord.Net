using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class Error
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
}
