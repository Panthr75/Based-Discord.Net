using System;
using System.Text.Json.Serialization;

namespace Discord.API.AuditLogs;

internal class MemberInfoAuditLogModel : IAuditLogInfoModel
{
    [JsonPropertyName("nick")]
    public string? Nickname { get; set; }

    [JsonPropertyName("mute")]
    public bool? IsMuted { get; set; }

    [JsonPropertyName("deaf")]
    public bool? IsDeafened { get; set; }

    [JsonPropertyName("communication_disabled_until")]
    public DateTimeOffset? TimeOutUntil { get; set; }
}
