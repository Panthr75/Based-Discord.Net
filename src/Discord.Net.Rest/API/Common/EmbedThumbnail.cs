using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class EmbedThumbnail
    {
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
        [JsonPropertyName("proxy_url")]
        public string ProxyUrl { get; set; } = string.Empty;
        [JsonPropertyName("height")]
        public Optional<int> Height { get; set; }
        [JsonPropertyName("width")]
        public Optional<int> Width { get; set; }
    }
}
