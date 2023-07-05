using System;
using System.Text.Json.Serialization;

namespace Discord.API.AuditLogs;

public class GuildInfoAuditLogModel : IAuditLogInfoModel
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("afk_timeout")]
    public int? AfkTimeout { get; set; }

    [JsonPropertyName("widget_enabled")]
    public bool? IsEmbeddable { get; set; }

    [JsonPropertyName("default_message_notifications")]
    public DefaultMessageNotifications? DefaultMessageNotifications { get; set; }

    [JsonPropertyName("mfa_level")]
    public MfaLevel? MfaLevel { get; set; }

    [JsonPropertyName("verification_level")]
    public VerificationLevel? VerificationLevel { get; set; }

    [JsonPropertyName("explicit_content_filter")]
    public ExplicitContentFilterLevel? ExplicitContentFilterLevel { get; set; }

    [JsonPropertyName("icon_hash")]
    public string? IconHash { get; set; }

    [JsonPropertyName("discovery_splash")]
    public string? DiscoverySplash { get; set; }

    [JsonPropertyName("splash")]
    public string? Splash { get; set; }

    [JsonPropertyName("afk_channel_id")]
    public ulong? AfkChannelId { get; set; }

    [JsonPropertyName("widget_channel_id")]
    public ulong? EmbeddedChannelId { get; set; }

    [JsonPropertyName("system_channel_id")]
    public ulong? SystemChannelId { get; set; }

    [JsonPropertyName("rules_channel_id")]
    public ulong? RulesChannelId { get; set; }

    [JsonPropertyName("public_updates_channel_id")]
    public ulong? PublicUpdatesChannelId { get; set; }

    [JsonPropertyName("owner_id")]
    public ulong? OwnerId { get; set; }

    [JsonPropertyName("application_id")]
    public ulong? ApplicationId { get; set; }

    [JsonPropertyName("region")]
    public string? RegionId { get; set; }

    [JsonPropertyName("banner")]
    public string? Banner { get; set; }

    [JsonPropertyName("vanity_url_code")]
    public string? VanityUrl { get; set; }

    [JsonPropertyName("system_channel_flags")]
    public SystemChannelMessageDeny? SystemChannelFlags { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("preferred_locale")]
    public string? PreferredLocale { get; set; }

    [JsonPropertyName("nsfw_level")]
    public NsfwLevel? NsfwLevel { get; set; }

    [JsonPropertyName("premium_progress_bar_enabled")]
    public bool? ProgressBarEnabled { get; set; }

}
