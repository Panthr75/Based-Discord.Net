using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System;

namespace Discord.API.Gateway
{
    internal class MessageDeleteBulkEvent
    {
        [JsonPropertyName("channel_id")]
        public ulong ChannelId { get; set; }
        [JsonPropertyName("ids")]
        public ulong[] Ids { get; set; } = Array.Empty<ulong>();
    }
}
