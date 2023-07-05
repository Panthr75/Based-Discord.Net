using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Rest
{
    internal class CreateDMChannelParams
    {
        [JsonPropertyName("recipient_id")]
        public ulong RecipientId { get; }

        public CreateDMChannelParams(ulong recipientId)
        {
            RecipientId = recipientId;
        }
    }
}
