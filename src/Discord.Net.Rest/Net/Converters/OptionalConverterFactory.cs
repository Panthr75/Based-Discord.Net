using System.Text.Json;
using System.Text.Json.Serialization;
using System;

namespace Discord.Net.Converters
{
    internal class OptionalConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Optional<>);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Type type = typeof(Converter<>).MakeGenericType(typeToConvert.GetGenericArguments()[0]);

            return (JsonConverter?)Activator.CreateInstance(type);
        }


        private sealed class Converter<T> : JsonConverter<Optional<T>>
        {
            public override Optional<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return JsonSerializer.Deserialize<T>(ref reader, options)!;
            }

            public override void Write(Utf8JsonWriter writer, Optional<T> value, JsonSerializerOptions options)
            {
                if (value.IsSpecified)
                {
                    JsonSerializer.Serialize(writer, value.Value, options);
                }
                else
                {
                    writer.WriteNullValue();
                }
            }
        }
    }
}
