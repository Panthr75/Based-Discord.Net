using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class AutocompleteInteractionDataOption
    {
        [JsonPropertyName("type")]
        public ApplicationCommandOptionType Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("options")]
        public Optional<AutocompleteInteractionDataOption[]> Options { get; set; }

        [JsonPropertyName("value")]
        public Optional<ApplicationCommandOptionValue> Value { get; set; }

        [JsonPropertyName("focused")]
        public Optional<bool> Focused { get; set; }
    }
}
