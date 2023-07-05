using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class AuditLog
    {
        [JsonPropertyName("webhooks")]
        public Webhook[] Webhooks { get; set; } = Array.Empty<Webhook>();

        [JsonPropertyName("threads")]
        public Channel[] Threads { get; set; } = Array.Empty<Channel>();

        [JsonPropertyName("integrations")]
        public Integration[] Integrations { get; set; } = Array.Empty<Integration>();

        [JsonPropertyName("users")]
        public User[] Users { get; set; } = Array.Empty<User>();

        [JsonPropertyName("audit_log_entries")]
        public AuditLogEntry[] Entries { get; set; } = Array.Empty<AuditLogEntry>();

        [JsonPropertyName("application_commands")]
        public ApplicationCommand[] Commands { get; set; } = Array.Empty<ApplicationCommand>();

        [JsonPropertyName("auto_moderation_rules")]
        public AutoModerationRule[] AutoModerationRules { get; set; } = Array.Empty<AutoModerationRule>();

        [JsonPropertyName("guild_scheduled_events")]
        public GuildScheduledEvent[] GuildScheduledEvents { get; set; } = Array.Empty<GuildScheduledEvent>();
    }
}
