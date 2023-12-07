using System.Text.Json.Serialization;

namespace Discord.API.Rest;

internal class ModifyVoiceStatusParams
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }
}
