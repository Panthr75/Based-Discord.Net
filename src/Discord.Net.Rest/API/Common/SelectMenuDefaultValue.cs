using System.Text.Json.Serialization;

namespace Discord.API;

internal class SelectMenuDefaultValue
{
    [JsonPropertyName("id")]
    public ulong Id { get; set; }

    [JsonPropertyName("type")]
    public SelectDefaultValueType Type { get; set; }
}
