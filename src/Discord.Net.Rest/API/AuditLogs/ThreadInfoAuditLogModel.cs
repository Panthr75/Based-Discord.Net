using System.Text.Json.Serialization;

namespace Discord.API.AuditLogs;

internal class ThreadInfoAuditLogModel : IAuditLogInfoModel
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type")]
    public ThreadType Type { get; set; }

    [JsonPropertyName("archived")]
    public bool? IsArchived { get; set; }

    [JsonPropertyName("locked")]
    public bool? IsLocked { get; set;}

    [JsonPropertyName("auto_archive_duration")]
    public ThreadArchiveDuration? ArchiveDuration { get; set; }

    [JsonPropertyName("rate_limit_per_user")]
    public int? SlowModeInterval { get; set; }

    [JsonPropertyName("flags")]
    public ChannelFlags? ChannelFlags { get; set; }

    [JsonPropertyName("applied_tags")]
    public ulong[]? AppliedTags { get; set; }
}
