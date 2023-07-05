using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Gateway
{
    internal class GuildEmojiUpdateEvent
    {
        [JsonPropertyName("guild_id")]
        public ulong GuildId { get; set; }
        [JsonPropertyName("emojis")]
        public Emoji[] Emojis { get; set; } = Array.Empty<Emoji>();
    }
}
