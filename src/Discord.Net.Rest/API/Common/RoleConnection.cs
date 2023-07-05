using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Discord.API;

public class RoleConnection
{
    [JsonPropertyName("platform_name")]
    public Optional<string> PlatformName { get; set; }

    [JsonPropertyName("platform_username")]
    public Optional<string> PlatformUsername { get; set; }

    [JsonPropertyName("metadata")]
    public Optional<Dictionary<string, string>> Metadata { get; set; }
}
