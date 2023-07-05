using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class UserGuild
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("icon")]
        public string? Icon { get; set; }
        [JsonPropertyName("owner")]
        public bool Owner { get; set; }
        [JsonPropertyName("permissions")]
        public string Permissions { get; set; } = string.Empty;
        [JsonPropertyName("features")]
        public GuildFeatures Features { get; set; } = null!;

        [JsonPropertyName("approximate_member_count")]
        public Optional<int> ApproximateMemberCount { get; set; }

        [JsonPropertyName("approximate_presence_count")]
        public Optional<int> ApproximatePresenceCount { get; set; }
    }
}
