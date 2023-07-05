using System.Text.Json.Serialization;

namespace Discord.API.AuditLogs;

internal class InviteInfoAuditLogModel : IAuditLogInfoModel
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("channel_id")]
    public ulong? ChannelId { get; set; }

    [JsonPropertyName("inviter_id")]
    public ulong? InviterId { get; set; }

    [JsonPropertyName("uses")]
    public int? Uses { get; set; }

    [JsonPropertyName("max_uses")]
    public int? MaxUses { get; set; }

    [JsonPropertyName("max_age")]
    public int? MaxAge { get; set; }

    [JsonPropertyName("temporary")]
    public bool? Temporary { get; set; }
}
