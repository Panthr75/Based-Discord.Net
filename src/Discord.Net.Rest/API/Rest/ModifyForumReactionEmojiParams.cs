using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API;

public class ModifyForumReactionEmojiParams
{
    [JsonPropertyName("emoji_id")]
    public Optional<ulong?> EmojiId { get; set; }

    [JsonPropertyName("emoji_name")]
    public Optional<string> EmojiName { get; set; }
}


