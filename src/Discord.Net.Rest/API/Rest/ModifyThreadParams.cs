using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Discord.API.Rest
{
    internal class ModifyThreadParams
    {
        [JsonPropertyName("name")]
        public Optional<string> Name { get; set; }

        [JsonPropertyName("archived")]
        public Optional<bool> Archived { get; set; }

        [JsonPropertyName("auto_archive_duration")]
        public Optional<ThreadArchiveDuration> AutoArchiveDuration { get; set; }

        [JsonPropertyName("locked")]
        public Optional<bool> Locked { get; set; }

        [JsonPropertyName("rate_limit_per_user")]
        public Optional<int> Slowmode { get; set; }

        [JsonPropertyName("applied_tags")]
        public Optional<IEnumerable<ulong>> AppliedTags { get; set; }

        [JsonPropertyName("flags")]
        public Optional<ChannelFlags> Flags { get; set; }
    }
}
