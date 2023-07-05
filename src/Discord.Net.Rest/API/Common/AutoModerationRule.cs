using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.API
{
    internal class AutoModerationRule
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("guild_id")]
        public ulong GuildId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("creator_id")]
        public ulong CreatorId { get; set; }

        [JsonPropertyName("event_type")]
        public AutoModEventType EventType { get; set; }

        [JsonPropertyName("trigger_type")]
        public AutoModTriggerType TriggerType { get; set; }

        [JsonPropertyName("trigger_metadata")]
        public TriggerMetadata TriggerMetadata { get; set; } = null!;

        [JsonPropertyName("actions")]
        public AutoModAction[] Actions { get; set; } = Array.Empty<AutoModAction>();

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        [JsonPropertyName("exempt_roles")]
        public ulong[] ExemptRoles { get; set; } = Array.Empty<ulong>();

        [JsonPropertyName("exempt_channels")]
        public ulong[] ExemptChannels { get; set; } = Array.Empty<ulong>();
    }
}
