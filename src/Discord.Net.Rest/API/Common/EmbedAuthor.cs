using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class EmbedAuthor
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("url")]
        public string? Url { get; set; }
        [JsonPropertyName("icon_url")]
        public string? IconUrl { get; set; }
        [JsonPropertyName("proxy_icon_url")]
        public string? ProxyIconUrl { get; set; }
    }
}
