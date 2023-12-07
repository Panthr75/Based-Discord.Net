using System.Text.Json.Serialization;

namespace Discord.API.Gateway;

internal class VoiceChannelStatusUpdateEvent
{
    [JsonPropertyName("id")]
    public ulong Id { get; set; }

    [JsonPropertyName("guild_id")]
    public ulong GuildId { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }
}
