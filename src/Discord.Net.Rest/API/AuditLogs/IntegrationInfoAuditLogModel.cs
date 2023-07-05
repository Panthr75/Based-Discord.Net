using System.Text.Json.Serialization;

namespace Discord.API.AuditLogs;

internal class IntegrationInfoAuditLogModel : IAuditLogInfoModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    [JsonPropertyName("syncing")]
    public bool? Syncing { get; set; }

    [JsonPropertyName("role_id")]
    public ulong? RoleId { get; set; }

    [JsonPropertyName("enable_emoticons")]
    public bool? EnableEmojis { get; set; }

    [JsonPropertyName("expire_behavior")]
    public IntegrationExpireBehavior? ExpireBehavior { get; set; }

    [JsonPropertyName("expire_grace_period")]
    public int? ExpireGracePeriod { get; set; }

    [JsonPropertyName("scopes")]
    public string[]? Scopes { get; set; }
}
