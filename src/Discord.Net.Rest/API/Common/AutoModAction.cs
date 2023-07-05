using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.API
{
    internal class AutoModAction
    {
        [JsonPropertyName("type")]
        public AutoModActionType Type { get; set; }

        [JsonPropertyName("metadata")]
        public Optional<ActionMetadata> Metadata { get; set; }
    }
}
