using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Discord.API
{
    internal class MessageComponentInteractionData : IDiscordInteractionData
    {
        [JsonPropertyName("custom_id")]
        public string CustomId { get; set; } = string.Empty;

        [JsonPropertyName("component_type")]
        public ComponentType ComponentType { get; set; }

        [JsonPropertyName("values")]
        public Optional<string[]> Values { get; set; }

        [JsonPropertyName("value")]
        public Optional<string> Value { get; set; }

        [JsonPropertyName("resolved")]
        public Optional<MessageComponentInteractionDataResolved> Resolved { get; set; }
    }
}
