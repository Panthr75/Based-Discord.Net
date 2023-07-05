using System.Text.Json;
using System.Text.Json.Serialization;
using System;

namespace Discord.Net.Converters
{
    internal sealed class UInt64EntityConverter : JsonConverter<IEntity<ulong>>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IEntity<ulong>).IsAssignableFrom(typeToConvert);
        }

        public override IEntity<ulong>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new InvalidOperationException();
        }

        public override void Write(Utf8JsonWriter writer, IEntity<ulong> value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.Id);
        }
    }
}
