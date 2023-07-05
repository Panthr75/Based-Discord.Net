using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.API.Rest
{
    internal class CreateAutoModRuleParams
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("event_type")]
        public AutoModEventType EventType { get; set; }

        [JsonPropertyName("trigger_type")]
        public AutoModTriggerType TriggerType { get; set; }

        [JsonPropertyName("trigger_metadata")]
        public Optional<TriggerMetadata> TriggerMetadata { get; set; }

        [JsonPropertyName("actions")]
        public AutoModAction[] Actions { get; set; } = Array.Empty<AutoModAction>();

        [JsonPropertyName("enabled")]
        public Optional<bool> Enabled { get; set; }

        [JsonPropertyName("exempt_roles")]
        public Optional<ulong[]> ExemptRoles { get; set; }

        [JsonPropertyName("exempt_channels")]
        public Optional<ulong[]> ExemptChannels { get; set; }
    }
}
