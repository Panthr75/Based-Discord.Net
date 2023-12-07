using System.Text.Json.Serialization;

namespace Discord.API;

internal class AvatarDecorationData
{
    [JsonPropertyName("asset")]
    public string Asset { get; set; } = string.Empty;

    [JsonPropertyName("sku_id")]
    public ulong SkuId { get; set; }
}
