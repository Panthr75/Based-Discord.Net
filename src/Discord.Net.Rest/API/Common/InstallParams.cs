using System.Text.Json;
using System.Text.Json.Serialization;
using System;

namespace Discord.API;

internal class InstallParams
{
    [JsonPropertyName("scopes")]
    public string[] Scopes { get; set; } = Array.Empty<string>();
    [JsonPropertyName("permissions")]
    public ulong Permission { get; set; }
}
