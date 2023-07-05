using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Gateway;

internal class AuditLogCreatedEvent : AuditLogEntry
{
    [JsonPropertyName("guild_id")]
    public ulong GuildId { get; set; }
}
