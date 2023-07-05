using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API;

internal class WelcomeScreenChannel
{
    [JsonPropertyName("channel_id")]
    public ulong ChannelId { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("emoji_id")]
    public Optional<ulong?> EmojiId { get; set; }

    [JsonPropertyName("emoji_name")]
    public Optional<string> EmojiName { get; set; }
}
