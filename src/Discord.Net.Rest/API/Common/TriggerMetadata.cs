using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.API
{
    internal class TriggerMetadata
    {
        [JsonPropertyName("keyword_filter")]
        public Optional<string[]> KeywordFilter { get; set; }

        [JsonPropertyName("regex_patterns")]
        public Optional<string[]> RegexPatterns { get; set; }

        [JsonPropertyName("presets")]
        public Optional<KeywordPresetTypes[]> Presets { get; set; }

        [JsonPropertyName("allow_list")]
        public Optional<string[]> AllowList { get; set; }

        [JsonPropertyName("mention_total_limit")]
        public Optional<int> MentionLimit { get; set; }
    }
}
