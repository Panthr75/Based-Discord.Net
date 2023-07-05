using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.API
{
    internal class ForumTag
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("emoji_id")]
        public Optional<ulong?> EmojiId { get; set; }
        [JsonPropertyName("emoji_name")]
        public Optional<string?> EmojiName { get; set; }

        [JsonPropertyName("moderated")]
        public bool Moderated { get; set; }
    }
}
