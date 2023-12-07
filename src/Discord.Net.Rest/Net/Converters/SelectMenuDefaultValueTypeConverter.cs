using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.Net.Converters;

internal class SelectMenuDefaultValueTypeConverter : JsonConverter<SelectDefaultValueType>
{
    public override SelectDefaultValueType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!Enum.TryParse<SelectDefaultValueType>(reader.GetString(), true, out var result))
        {
#if DEBUG
            System.Diagnostics.Debug.Fail("Failed to parse enum value " + reader.GetString());
#endif
        }
        return result;
    }

    public override void Write(Utf8JsonWriter writer, SelectDefaultValueType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString().ToLower(CultureInfo.InvariantCulture));
    }
}
