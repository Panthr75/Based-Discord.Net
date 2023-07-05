using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Rest
{
    internal class GuildPruneParams
    {
        [JsonPropertyName("days")]
        public int Days { get; }

        [JsonPropertyName("include_roles")]
        public ulong[] IncludeRoleIds { get; }

        public GuildPruneParams(int days, ulong[] includeRoleIds)
        {
            Days = days;
            IncludeRoleIds = includeRoleIds;
        }
    }
}
