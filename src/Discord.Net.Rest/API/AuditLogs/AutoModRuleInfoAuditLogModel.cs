using System.Text.Json.Serialization;

namespace Discord.API.AuditLogs;

internal class AutoModRuleInfoAuditLogModel : IAuditLogInfoModel
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("event_type")]
    public AutoModEventType? EventType { get; set; }

    [JsonPropertyName("trigger_type")]
    public AutoModTriggerType? TriggerType { get; set; }

    [JsonPropertyName("trigger_metadata")]
    public TriggerMetadata? TriggerMetadata { get; set; } = new();

    [JsonPropertyName("actions")]
    public AutoModAction[]? Actions { get; set; }

    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    [JsonPropertyName("exempt_roles")]
    public ulong[]? ExemptRoles { get; set; }

    [JsonPropertyName("exempt_channels")]
    public ulong[]? ExemptChannels { get; set; }
}
