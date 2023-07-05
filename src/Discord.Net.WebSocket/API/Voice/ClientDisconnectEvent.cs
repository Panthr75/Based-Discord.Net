using System.Text.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Discord.API.Voice;
internal class ClientDisconnectEvent
{
    [JsonPropertyName("user_id")]
    public ulong UserId { get; set; }
}
