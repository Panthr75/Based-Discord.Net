using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class ApplicationCommandInteractionDataOption
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public ApplicationCommandOptionType Type { get; set; }

        [JsonPropertyName("value")]
        public Optional<ApplicationCommandOptionValue> Value { get; set; }

        [JsonPropertyName("options")]
        public Optional<ApplicationCommandInteractionDataOption[]> Options { get; set; }
    }
}
