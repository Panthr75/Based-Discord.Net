using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.Net.Converters;

internal sealed class EnumStringValueConverter<T> : JsonConverter<T>
    where T : struct, Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
#if NETSTANDARD2_0
        return (T)Enum.Parse(typeof(T), reader.GetString()!, true);
#else
        return Enum.Parse<T>(reader.GetString()!, true);
#endif
    }
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("D"));
    }
}
