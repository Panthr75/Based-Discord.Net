using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class ModalInteractionData : IDiscordInteractionData
    {
        [JsonPropertyName("custom_id")]
        public string CustomId { get; set; } = string.Empty;

        [JsonPropertyName("components")]
        public API.ActionRowComponent[] Components { get; set; } = Array.Empty<API.ActionRowComponent>();
    }
}
