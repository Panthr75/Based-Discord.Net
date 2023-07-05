using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Rest
{
    internal class ModifyVoiceChannelParams : ModifyTextChannelParams
    {
        [JsonPropertyName("bitrate")]
        public Optional<int> Bitrate { get; set; }
        [JsonPropertyName("user_limit")]
        public Optional<int> UserLimit { get; set; }
        [JsonPropertyName("rtc_region")]
        public Optional<string> RTCRegion { get; set; }
    }
}
