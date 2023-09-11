using System.Text.Json.Serialization;

namespace Discord.API;

internal class Webhook
{
    [JsonPropertyName("id")]
    public ulong Id { get; set; }

    [JsonPropertyName("type")]
    public WebhookType Type { get; set; }

    [JsonPropertyName("guild_id")]
    public Optional<ulong?> GuildId { get; set; }

    [JsonPropertyName("channel_id")]
    public ulong? ChannelId { get; set; }

    [JsonPropertyName("user")]
    public Optional<User> Creator { get; set; }

    [JsonPropertyName("name")]
    public Optional<string> Name { get; set; }

    [JsonPropertyName("avatar")]
    public Optional<string> Avatar { get; set; }

    [JsonPropertyName("token")]
    public Optional<string> Token { get; set; }

    [JsonPropertyName("application_id")]
    public ulong? ApplicationId { get; set; }

    [JsonPropertyName("source_guild")]
    public Optional<PartialGuild> Guild { get; set; }

    [JsonPropertyName("source_channel")]
    public Optional<Channel> Channel { get; set; }
}
