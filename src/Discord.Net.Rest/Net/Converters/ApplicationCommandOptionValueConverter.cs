using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.Net.Converters;

internal sealed class ApplicationCommandOptionValueConverter : JsonConverter<ApplicationCommandOptionValue>
{
    public override ApplicationCommandOptionValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return ApplicationCommandOptionValue.None;
        }
        else if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString()!;
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return JsonSerializer.Deserialize<NumericValue>(ref reader, options);
        }
        else if (reader.TokenType == JsonTokenType.True || reader.TokenType == JsonTokenType.False)
        {
            return reader.GetBoolean();
        }

        throw new InvalidOperationException("Cannot convert given value to ApplicationCommandOptionValue");
    }

    public override void Write(Utf8JsonWriter writer, ApplicationCommandOptionValue value, JsonSerializerOptions options)
    {
        if (value.IsNone)
        {
            writer.WriteNullValue();
        }
        else if (value.IsBool)
        {
            writer.WriteBooleanValue(value.ToBool());
        }
        else if (value.IsNumberLike)
        {
            JsonSerializer.Serialize(writer, value.ToNumber(), options);
        }
        else if (value.IsStringLike)
        {
            writer.WriteStringValue(value.ToString());
        }
        else
        {
            throw new NotSupportedException();
        }
    }
}
