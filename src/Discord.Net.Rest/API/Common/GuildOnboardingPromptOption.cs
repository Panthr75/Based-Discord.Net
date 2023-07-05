using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API;

internal class GuildOnboardingPromptOption
{
    [JsonPropertyName("id")]
    public ulong Id { get; set; }

    [JsonPropertyName("channel_ids")]
    public ulong[] ChannelIds { get; set; } = Array.Empty<ulong>();

    [JsonPropertyName("role_ids")]
    public ulong[] RoleIds { get; set; } = Array.Empty<ulong>();

    [JsonPropertyName("emoji")]
    public Emoji Emoji { get; set; } = null!;

    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [JsonPropertyName("description")]
    public Optional<string> Description { get; set; }
}
