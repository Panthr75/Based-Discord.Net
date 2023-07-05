using System;
using System.Text.Json.Serialization;

namespace Discord.API;

internal class WelcomeScreen
{
    [JsonPropertyName("description")]
    public Optional<string> Description { get; set; }

    [JsonPropertyName("welcome_channels")]
    public WelcomeScreenChannel[] WelcomeChannels { get; set; } = Array.Empty<WelcomeScreenChannel>();
}
