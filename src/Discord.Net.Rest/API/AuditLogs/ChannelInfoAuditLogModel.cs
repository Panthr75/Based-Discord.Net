using Discord.Rest;
using System.Text.Json.Serialization;

namespace Discord.API.AuditLogs;

internal class ChannelInfoAuditLogModel : IAuditLogInfoModel
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type")]
    public ChannelType? Type { get; set; }

    [JsonPropertyName("permission_overwrites")]
    public Overwrite[]? Overwrites { get; set; }

    [JsonPropertyName("flags")]
    public ChannelFlags? Flags { get; set; }

    [JsonPropertyName("default_thread_rate_limit_per_user")]
    public int? DefaultThreadRateLimitPerUser { get; set; }

    [JsonPropertyName("default_auto_archive_duration")]
    public ThreadArchiveDuration? DefaultArchiveDuration { get; set; }

    [JsonPropertyName("rate_limit_per_user")]
    public int? RateLimitPerUser { get; set; }

    [JsonPropertyName("auto_archive_duration")]
    public ThreadArchiveDuration? AutoArchiveDuration { get; set; }

    [JsonPropertyName("nsfw")]
    public bool? IsNsfw { get; set; }

    [JsonPropertyName("topic")]
    public string? Topic { get; set; }

    // Forum channels
    [JsonPropertyName("available_tags")]
    public ForumTag[]? AvailableTags { get; set; }

    [JsonPropertyName("default_reaction_emoji")]
    public ForumReactionEmoji? DefaultEmoji { get; set; }

    // Voice channels

    [JsonPropertyName("user_limit")]
    public int? UserLimit { get; set; }

    [JsonPropertyName("rtc_region")]
    public string? Region { get; set; }

    [JsonPropertyName("video_quality_mode")]
    public VideoQualityMode? VideoQualityMode { get; set; }

    [JsonPropertyName("bitrate")]
    public int? Bitrate { get; set; }
}
