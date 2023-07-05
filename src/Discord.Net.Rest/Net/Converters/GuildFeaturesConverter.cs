using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Discord.Net.Converters
{
    internal class GuildFeaturesConverter : JsonConverter<GuildFeatures>
    {
        public override GuildFeatures? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string[]? featuresArray = JsonSerializer.Deserialize<string[]>(ref reader, options);
            if (featuresArray == null)
            {
                return null;
            }

            List<string> experimental = new();

            GuildFeature features = featuresArray.Select(feature =>
            {
                if (!Enum.TryParse(string.Concat(feature.Split('_')), true, out GuildFeature result))
                {
                    experimental.Add(feature);
                    return GuildFeature.None;
                }
                return result;
            }).Aggregate(GuildFeature.None, (a, b) => a | b);

            return new GuildFeatures(features, experimental.ToArray());
        }

        public override void Write(Utf8JsonWriter writer, GuildFeatures value, JsonSerializerOptions options)
        {
            GuildFeature v = value.Value;
#if NETSTANDARD2_0 || NETSTANDARD2_1
            string[] allFeatures = Enum.GetValues(typeof(GuildFeature)).Cast<GuildFeature>()
#else
            string[] allFeatures = Enum.GetValues<GuildFeature>()
#endif
                .Where(f => f != GuildFeature.None && v.HasFlag(f))
                .Select(f => FeatureToApiString(f))
                .ToArray();

            JsonSerializer.Serialize(writer, allFeatures, options);

        }

        private static string FeatureToApiString(GuildFeature feature)
        {
            var builder = new StringBuilder();
            var firstChar = true;

            foreach (var c in feature.ToString().ToCharArray())
            {
                if (char.IsUpper(c))
                {
                    if (firstChar)
                        firstChar = false;
                    else
                        builder.Append('_');

                    builder.Append(c);
                }
                else
                {
                    builder.Append(char.ToUpper(c));
                }
            }

            return builder.ToString();
        }
    }
}
