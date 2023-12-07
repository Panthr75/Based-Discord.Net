using System.Text.Json.Serialization;

namespace Discord.API;

internal class GuildInventorySettings
{
    [JsonPropertyName("is_emoji_pack_collectible")]
    public Optional<bool> IsEmojiPackCollectible { get; set; }
}
