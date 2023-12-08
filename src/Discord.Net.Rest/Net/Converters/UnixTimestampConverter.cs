using System.Text.Json;
using System.Text.Json.Serialization;
using System;

namespace Discord.Net.Converters
{
    internal sealed class UnixTimestampConverter : JsonConverter<Optional<DateTimeOffset>>
    {
        // 1e13 unix ms = year 2286
        // necessary to prevent discord.js from sending values in the e15 and overflowing a DTO
        private const long MaxSaneMs = 1_000_000_000_000_0;

        private static readonly DateTimeOffset Epoch = new(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);

        public override Optional<DateTimeOffset> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            long? value = null;
            if (reader.TokenType == JsonTokenType.Number)
            {
                value = reader.GetInt64();
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                if (long.TryParse(reader.GetString(), out long v))
                {
                    value = v;
                }
            }

            if (value.HasValue)
            {
                long v = value.Value;
                // Discord doesn't validate if timestamps contain decimals or not, and they also don't validate if timestamps are reasonably sized
                if (v >= MaxSaneMs)
                {
                    return Optional<DateTimeOffset>.Unspecified;
                }

                return Epoch.AddMilliseconds(v);
            }

            // try parsing as normal date time offset
            try
            {
                return reader.GetDateTimeOffset();
            }
            // dont catch invalid operation exception, as datetimeoffsets can
            // only be parsed from json token type of string.
            catch (FormatException)
            {
                return Optional<DateTimeOffset>.Unspecified;
            }
        }

        public override void Write(Utf8JsonWriter writer, Optional<DateTimeOffset> value, JsonSerializerOptions options)
        {
            if (!value.IsSpecified)
            {
                writer.WriteNullValue();
                return;
            }

            TimeSpan time = value.Value - Epoch;

            writer.WriteNumberValue((long)time.TotalMilliseconds);
        }
    }
}
