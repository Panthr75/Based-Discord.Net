using System.Text.Json.Serialization;

namespace Discord.API.AuditLogs;

internal class OnboardingAuditLogModel : IAuditLogInfoModel
{
    [JsonPropertyName("default_channel_ids")]
    public ulong[]? DefaultChannelIds { get; set; }

    [JsonPropertyName("prompts")]
    public GuildOnboardingPrompt[]? Prompts { get; set; }

    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }
}
