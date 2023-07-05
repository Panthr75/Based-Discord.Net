using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Discord.API
{
    internal class ApplicationCommandOptionChoice
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("value")]
        public ApplicationCommandOptionValue Value { get; set; } = string.Empty;

        [JsonPropertyName("name_localizations")]
        public Optional<Dictionary<string, string>> NameLocalizations { get; set; }

        [JsonPropertyName("name_localized")]
        public Optional<string> NameLocalized { get; set; }
    }
}
