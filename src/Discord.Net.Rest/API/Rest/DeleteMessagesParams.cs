using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Rest
{
    internal class DeleteMessagesParams
    {
        [JsonPropertyName("messages")]
        public ulong[] MessageIds { get; }

        public DeleteMessagesParams(ulong[] messageIds)
        {
            MessageIds = messageIds;
        }
    }
}
