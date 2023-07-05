using System.Text.Json;
using System.Text.Json.Serialization;
using System;

namespace Discord.Net.Converters
{
    internal sealed class StringEntityConverter : JsonConverter<IEntity<string>>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IEntity<string>).IsAssignableFrom(typeToConvert);
        }

        public override IEntity<string>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new InvalidOperationException();
        }

        public override void Write(Utf8JsonWriter writer, IEntity<string> value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Id);
        }
    }
}
