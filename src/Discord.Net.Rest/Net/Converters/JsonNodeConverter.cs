using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Discord.Net.Converters
{
    internal sealed class JsonNodeConverter : JsonConverter<JsonNode>
    {
        public override JsonNode? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonNode.Parse(ref reader, new JsonNodeOptions()
                {
                    PropertyNameCaseInsensitive = options.PropertyNameCaseInsensitive
                });
        }
        public override void Write(Utf8JsonWriter writer, JsonNode value, JsonSerializerOptions options)
        {
            value.WriteTo(writer, options);
        }
    }
}
