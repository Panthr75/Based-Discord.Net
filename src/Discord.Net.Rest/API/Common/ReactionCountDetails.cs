using System.Text.Json.Serialization;

namespace Discord.API;

internal class ReactionCountDetails
{
    [JsonPropertyName("normal")]
    public int NormalCount { get; set; }

    [JsonPropertyName("burst")]
    public int BurstCount { get; set; }
}
