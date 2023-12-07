using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Gateway;

internal class Reaction
{
    [JsonPropertyName("user_id")]
    public ulong UserId { get; set; }

    [JsonPropertyName("message_id")]
    public ulong MessageId { get; set; }

    [JsonPropertyName("channel_id")]
    public ulong ChannelId { get; set; }

    [JsonPropertyName("guild_id")]
    public Optional<ulong> GuildId { get; set; }

    [JsonPropertyName("emoji")]
    public Emoji Emoji { get; set; } = null!;

    [JsonPropertyName("member")]
    public Optional<GuildMember> Member { get; set; }

    [JsonPropertyName("burst")]
    public bool IsBurst { get; set; }

    [JsonPropertyName("burst_colors")]
    public Optional<string[]> BurstColors { get; set; }

    [JsonPropertyName("type")]
    public ReactionType Type { get; set; }
}
