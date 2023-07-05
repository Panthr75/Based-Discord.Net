using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API;

internal class FollowedChannel
{
    [JsonPropertyName("channel_id")]
    public ulong ChannelId { get; set; }

    [JsonPropertyName("webhook_id")]
    public ulong WebhookId { get; set; }
}
