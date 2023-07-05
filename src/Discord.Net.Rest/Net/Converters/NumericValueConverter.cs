using Discord.Utils;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.Net.Converters
{
    internal sealed class NumericValueConverter : JsonConverter<NumericValue>
    {
        public override NumericValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            double doubleValue = reader.GetDouble();
            try
            {
                long integerValue = reader.GetInt64();

                return new NumericValue(integerValue, doubleValue);
            }
            catch
            {
                return new NumericValue(doubleValue);
            }
        }

        public override void Write(Utf8JsonWriter writer, NumericValue value, JsonSerializerOptions options)
        {
            if (value.IsIntegerAndDouble)
            {
                // no decimal, less than long.MaxValue, greater than
                // long.MinValue, use integer part
                if (Math.Round(value.UnderlyingDouble) == value.UnderlyingDouble &&
                    value.UnderlyingDouble > long.MinValue &&
                    value.UnderlyingDouble < long.MaxValue)
                {
                    writer.WriteNumberValue(value.UnderlyingInteger);
                }
                else
                {
                    writer.WriteNumberValue(value.UnderlyingDouble);
                }
            }
            else if (value.IsDouble)
            {
                writer.WriteNumberValue(value.UnderlyingDouble);
            }
            else
            {
                writer.WriteNumberValue(value.UnderlyingInteger);
            }
        }
    }
}
