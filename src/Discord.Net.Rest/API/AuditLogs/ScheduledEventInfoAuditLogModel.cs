using System;
using System.Text.Json.Serialization;

namespace Discord.API.AuditLogs;

internal class ScheduledEventInfoAuditLogModel : IAuditLogInfoModel
{
    [JsonPropertyName("channel_id")]
    public ulong? ChannelId { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("scheduled_start_time")]
    public DateTimeOffset? StartTime { get; set; }

    [JsonPropertyName("scheduled_end_time")]
    public DateTimeOffset? EndTime { get; set; }

    [JsonPropertyName("privacy_level")]
    public GuildScheduledEventPrivacyLevel? PrivacyLevel { get; set; }

    [JsonPropertyName("status")]
    public GuildScheduledEventStatus? EventStatus { get; set; }

    [JsonPropertyName("entity_type")]
    public GuildScheduledEventType? EventType { get; set; }

    [JsonPropertyName("entity_id")]
    public ulong? EntityId { get; set; }
    
    [JsonPropertyName("user_count")]
    public int? UserCount { get; set; }

    [JsonPropertyName("location")]
    public string? Location { get; set; }

    [JsonPropertyName("image")]
    public string? Image { get; set; }
}
