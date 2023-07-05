using System.Text.Json;
using System;
using System.Text.Json.Serialization;

namespace Discord.API.Gateway
{
    internal class InviteCreateEvent
    {
        [JsonPropertyName("channel_id")]
        public ulong ChannelId { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;
        [JsonPropertyName("created_at")]
        public DateTimeOffset CreatedAt { get; set; }
        [JsonPropertyName("guild_id")]
        public Optional<ulong> GuildId { get; set; }
        [JsonPropertyName("inviter")]
        public Optional<User> Inviter { get; set; }
        [JsonPropertyName("max_age")]
        public int MaxAge { get; set; }
        [JsonPropertyName("max_uses")]
        public int MaxUses { get; set; }
        [JsonPropertyName("target_user")]
        public Optional<User> TargetUser { get; set; }
        [JsonPropertyName("target_type")]
        public Optional<TargetUserType> TargetUserType { get; set; }
        [JsonPropertyName("temporary")]
        public bool Temporary { get; set; }
        [JsonPropertyName("uses")]
        public int Uses { get; set; }

        [JsonPropertyName("target_application")]
        public Optional<Application> Application { get; set; }

        [JsonPropertyName("expires_at")]
        public Optional<DateTimeOffset?> ExpiresAt { get; set; }
    }
}
