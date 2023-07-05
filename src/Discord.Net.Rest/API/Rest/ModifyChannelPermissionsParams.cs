using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Rest
{
    internal class ModifyChannelPermissionsParams
    {
        [JsonPropertyName("type")]
        public int Type { get; }
        [JsonPropertyName("allow")]
        public string Allow { get; }
        [JsonPropertyName("deny")]
        public string Deny { get; }

        public ModifyChannelPermissionsParams(int type, string allow, string deny)
        {
            Type = type;
            Allow = allow;
            Deny = deny;
        }
    }
}
