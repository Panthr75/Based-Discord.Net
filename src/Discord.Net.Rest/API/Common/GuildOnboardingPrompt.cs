using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API;

internal class GuildOnboardingPrompt
{
    [JsonPropertyName("id")]
    public ulong Id { get; set; }

    [JsonPropertyName("options")]
    public GuildOnboardingPromptOption[] Options { get; set; } = Array.Empty<GuildOnboardingPromptOption>();

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("single_select")]
    public bool IsSingleSelect { get; set; }

    [JsonPropertyName("required")]
    public bool IsRequired { get; set; }

    [JsonPropertyName("in_onboarding")]
    public bool IsInOnboarding { get; set; }

    [JsonPropertyName("type")]
    public GuildOnboardingPromptType Type { get; set; }
}
