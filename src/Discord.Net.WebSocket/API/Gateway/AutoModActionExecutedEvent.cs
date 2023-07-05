using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Gateway;

internal class AutoModActionExecutedEvent
{
    [JsonPropertyName("guild_id")]
    public ulong GuildId { get; set; }

    [JsonPropertyName("action")]
    public Discord.API.AutoModAction Action { get; set; } = null!;

    [JsonPropertyName("rule_id")]
    public ulong RuleId { get; set; }

    [JsonPropertyName("rule_trigger_type")]
    public AutoModTriggerType TriggerType { get; set; }

    [JsonPropertyName("user_id")]
    public ulong UserId { get; set; }

    [JsonPropertyName("channel_id")]
    public Optional<ulong> ChannelId { get; set; }

    [JsonPropertyName("message_id")]
    public Optional<ulong> MessageId { get; set; }

    [JsonPropertyName("alert_system_message_id")]
    public Optional<ulong> AlertSystemMessageId { get; set; }

    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("matched_keyword")]
    public Optional<string> MatchedKeyword { get; set; }

    [JsonPropertyName("matched_content")]
    public Optional<string> MatchedContent { get; set; }
}
