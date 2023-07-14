using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API;

internal class GuildOnboarding
{
    [JsonPropertyName("guild_id")]
    public ulong GuildId { get; set; }

    [JsonPropertyName("prompts")]
    public GuildOnboardingPrompt[] Prompts { get; set; } = Array.Empty<GuildOnboardingPrompt>();

    [JsonPropertyName("default_channel_ids")]
    public ulong[] DefaultChannelIds { get; set; } = Array.Empty<ulong>();

    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }

    [JsonPropertyName("mode")]
    public GuildOnboardingMode Mode { get; set; }

    [JsonPropertyName("below_requirements")]
    public bool IsBelowRequirements { get; set; }
}
