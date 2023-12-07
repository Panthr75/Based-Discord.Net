using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API;

internal class Attachment
{
    [JsonPropertyName("id")]
    public ulong Id { get; set; }
    [JsonPropertyName("filename")]
    public string Filename { get; set; } = string.Empty;
    [JsonPropertyName("description")]
    public Optional<string> Description { get; set; }
    [JsonPropertyName("content_type")]
    public Optional<string> ContentType { get; set; }
    [JsonPropertyName("size")]
    public int Size { get; set; }
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
    [JsonPropertyName("proxy_url")]
    public string ProxyUrl { get; set; } = string.Empty;
    [JsonPropertyName("height")]
    public Optional<int> Height { get; set; }
    [JsonPropertyName("width")]
    public Optional<int> Width { get; set; }
    [JsonPropertyName("ephemeral")]
    public Optional<bool> Ephemeral { get; set; }
    [JsonPropertyName("duration_secs")]
    public Optional<double> DurationSeconds { get; set; }
    [JsonPropertyName("waveform")]
    public Optional<string> Waveform { get; set; }
    [JsonPropertyName("flags")]
    public Optional<AttachmentFlags> Flags { get; set; }
    [JsonPropertyName("title")]
    public Optional<string> Title { get; set; }
    [JsonPropertyName("clip_created_at")]
    public Optional<DateTimeOffset> ClipCreatedAt { get; set; }
    [JsonPropertyName("clip_participants")]
    public Optional<User[]> ClipParticipants { get; set; }
}
