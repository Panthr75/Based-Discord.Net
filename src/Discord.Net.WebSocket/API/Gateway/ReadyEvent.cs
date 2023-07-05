using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Gateway
{
    internal class ReadyEvent
    {
        public class ReadState
        {
            [JsonPropertyName("id")]
            public string ChannelId { get; set; } = string.Empty;
            [JsonPropertyName("mention_count")]
            public int MentionCount { get; set; }
            [JsonPropertyName("last_message_id")]
            public string LastMessageId { get; set; } = string.Empty;
        }

        [JsonPropertyName("v")]
        public int Version { get; set; }
        [JsonPropertyName("user")]
        public User User { get; set; } = null!;
        [JsonPropertyName("session_id")]
        public string SessionId { get; set; } = null!;
        [JsonPropertyName("resume_gateway_url")]
        public string ResumeGatewayUrl { get; set; } = null!;
        [JsonPropertyName("read_state")]
        public ReadState[] ReadStates { get; set; } = Array.Empty<ReadState>();
        [JsonPropertyName("guilds")]
        public ExtendedGuild[] Guilds { get; set; } = Array.Empty<ExtendedGuild>();
        [JsonPropertyName("private_channels")]
        public Channel[] PrivateChannels { get; set; } = Array.Empty<Channel>();
        [JsonPropertyName("relationships")]
        public Relationship[] Relationships { get; set; } = Array.Empty<Relationship>();
        [JsonPropertyName("application")]
        public PartialApplication Application { get; set; } = null!;

        //Ignored
        /*[JsonPropertyName("user_settings")]
        [JsonPropertyName("user_guild_settings")]
        [JsonPropertyName("tutorial")]*/
    }
}
