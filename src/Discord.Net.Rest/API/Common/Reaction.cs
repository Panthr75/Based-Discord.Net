using System;
using System.Text.Json.Serialization;

namespace Discord.API;

internal class Reaction
{
    [JsonPropertyName("count")]
    public int Count { get; set; }
    [JsonPropertyName("me")]
    public bool Me { get; set; }
    [JsonPropertyName("me_burst")]
    public bool MeBurst { get; set; }
    [JsonPropertyName("emoji")]
    public Emoji Emoji { get; set; } = null!;

    [JsonPropertyName("count_details")]
    public ReactionCountDetails CountDetails { get; set; } = null!;

    [JsonPropertyName("burst_colors")]
    public string[] BurstColors { get; set; } = Array.Empty<string>();
}
