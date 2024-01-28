using Discord.API;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Globalization;

namespace Discord.Net.Converters;

internal sealed class UInt64EntityOrIdConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(EntityOrId<>);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(Converter<>).MakeGenericType(typeToConvert.GetGenericArguments()[0]);

        return (JsonConverter?)Activator.CreateInstance(converterType);
    }

    private sealed class Converter<T> : JsonConverter<EntityOrId<T>>
    {
        public override EntityOrId<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new EntityOrId<T>(ulong.Parse(reader.GetString()!, NumberStyles.None, CultureInfo.InvariantCulture));
            }
            if (reader.TokenType == JsonTokenType.Number)
            {
                return new EntityOrId<T>(reader.GetUInt64());
            }

            var value = JsonSerializer.Deserialize<T>(ref reader, options);
            if (value is null)
            {
                return default;
            }

            return new EntityOrId<T>(value);
        }

        public override void Write(Utf8JsonWriter writer, EntityOrId<T> value, JsonSerializerOptions options)
        {
            if (value.IsObject)
            {
                JsonSerializer.Serialize(writer, value.Object, options);
            }
            else
            {
                writer.WriteNumberValue(value.Id);
            }
        }
    }
}
