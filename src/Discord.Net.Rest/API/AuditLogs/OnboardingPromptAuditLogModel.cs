using System.Text.Json.Serialization;

namespace Discord.API.AuditLogs;

internal class OnboardingPromptAuditLogModel : IAuditLogInfoModel
{
    [JsonPropertyName("id")]
    public ulong? Id { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("options")]
    public GuildOnboardingPromptOption[]? Options { get; set; }

    [JsonPropertyName("single_select")]
    public bool? IsSingleSelect { get; set; }

    [JsonPropertyName("required")]
    public bool? IsRequired { get; set; }

    [JsonPropertyName("in_onboarding")]
    public bool? IsInOnboarding { get; set; }

    [JsonPropertyName("type")]
    public GuildOnboardingPromptType? Type { get; set; }
}
