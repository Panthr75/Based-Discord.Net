using System.Text.Json.Serialization;

namespace Discord.API.AuditLogs;

internal class WebhookInfoAuditLogModel : IAuditLogInfoModel
{
    [JsonPropertyName("channel_id")]
    public ulong? ChannelId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type")]
    public WebhookType? Type { get; set; }

    [JsonPropertyName("avatar_hash")]
    public string? AvatarHash { get; set; }
}
