using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class Team
    {
        [JsonPropertyName("icon")]
        public Optional<string> Icon { get; set; }
        [JsonPropertyName("id")]
        public ulong Id { get; set; }
        [JsonPropertyName("members")]
        public TeamMember[] TeamMembers { get; set; } = Array.Empty<TeamMember>();
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("owner_user_id")]
        public ulong OwnerUserId { get; set; }
    }
}
