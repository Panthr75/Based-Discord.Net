using Discord.Rest;
using System.Text.Json.Serialization;

namespace Discord.API.AuditLogs;

internal class RoleInfoAuditLogModel : IAuditLogInfoModel
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("color")]
    public uint? Color { get; set; }

    [JsonPropertyName("hoist")]
    public bool? Hoist { get; set; }

    [JsonPropertyName("permissions")]
    public ulong? Permissions { get; set; }

    [JsonPropertyName("mentionable")]
    public bool? IsMentionable { get; set; }

    [JsonPropertyName("icon_hash")]
    public string? IconHash { get; set; }
}
