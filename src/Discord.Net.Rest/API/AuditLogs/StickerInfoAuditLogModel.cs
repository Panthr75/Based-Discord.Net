using System.Text.Json.Serialization;

namespace Discord.API.AuditLogs;

internal class StickerInfoAuditLogModel : IAuditLogInfoModel
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("tags")]
    public string? Tags { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
