using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API.Rest;

internal class ModifyCurrentApplicationBotParams
{
    [JsonPropertyName("interactions_endpoint_url")]
    public Optional<string> InteractionsEndpointUrl { get; set; }

    [JsonPropertyName("role_connections_verification_url")]
    public Optional<string> RoleConnectionsEndpointUrl { get; set; }

    [JsonPropertyName("description")]
    public Optional<string> Description { get; set; }

    [JsonPropertyName("tags")]
    public Optional<string[]> Tags { get; set; }

    [JsonPropertyName("icon")]
    public Optional<Image?> Icon { get; set; }
}
