using System.Text.Json.Serialization;

namespace Discord.API.Rest;

internal class CreateEntitlementParams
{
    [JsonPropertyName("sku_id")]
    public ulong SkuId { get; set; }

    [JsonPropertyName("owner_id")]
    public ulong OwnerId { get; set; }

    [JsonPropertyName("owner_type")]
    public SubscriptionOwnerType Type { get; set; }
}
