using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Rest;


internal class ModifyForumChannelParams : ModifyDiscussionChannelParams
{
    [JsonPropertyName("default_forum_layout")]
    public Optional<ForumLayout> DefaultLayout { get; set; }
}
