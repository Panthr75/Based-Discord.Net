using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Rest
{
    internal class ChannelThreads
    {
        [JsonPropertyName("threads")]
        public Channel[] Threads { get; set; } = Array.Empty<Channel>();

        [JsonPropertyName("members")]
        public ThreadMember[] Members { get; set; } = Array.Empty<ThreadMember>();
    }
}
