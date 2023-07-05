using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Discord.API;

public class RoleConnectionMetadata
{
    [JsonPropertyName("type")]
    public RoleConnectionMetadataType Type { get; set; }

    [JsonPropertyName("key")]
    public string Key { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("name_localizations")]
    public Optional<Dictionary<string, string>> NameLocalizations { get; set; }

    [JsonPropertyName("description_localizations")]
    public Optional<Dictionary<string, string>> DescriptionLocalizations { get; set; }
}
