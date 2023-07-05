using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System;

namespace Discord.API.Gateway
{
    internal class RequestMembersParams
    {
        [JsonPropertyName("query")]
        public string Query { get; set; } = string.Empty;
        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("guild_id")]
        public IEnumerable<ulong> GuildIds { get; set; } = Array.Empty<ulong>();
    }
}
