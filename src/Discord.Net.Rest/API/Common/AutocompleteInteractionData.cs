using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class AutocompleteInteractionData : IDiscordInteractionData
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public ApplicationCommandType Type { get; set; }

        [JsonPropertyName("version")]
        public ulong Version { get; set; }

        [JsonPropertyName("options")]
        public AutocompleteInteractionDataOption[] Options { get; set; } = Array.Empty<AutocompleteInteractionDataOption>();
    }
}
