using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.Net.Converters
{
    internal class EmbedTypeConverter : JsonConverter<EmbedType>
    {
        public static readonly EmbedTypeConverter Instance = new();

        public override EmbedType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString() switch
            {
                "rich" => EmbedType.Rich,
                "link" => EmbedType.Link,
                "video" => EmbedType.Video,
                "image" => EmbedType.Image,
                "gifv" => EmbedType.Gifv,
                "article" => EmbedType.Article,
                "tweet" => EmbedType.Tweet,
                "html" => EmbedType.Html,
                // TODO 2.2 EmbedType.News
                _ => EmbedType.Unknown,
            };
        }

        public override void Write(Utf8JsonWriter writer, EmbedType value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case EmbedType.Rich:
                    writer.WriteStringValue("rich");
                    break;
                case EmbedType.Link:
                    writer.WriteStringValue("link");
                    break;
                case EmbedType.Video:
                    writer.WriteStringValue("video");
                    break;
                case EmbedType.Image:
                    writer.WriteStringValue("image");
                    break;
                case EmbedType.Gifv:
                    writer.WriteStringValue("gifv");
                    break;
                case EmbedType.Article:
                    writer.WriteStringValue("article");
                    break;
                case EmbedType.Tweet:
                    writer.WriteStringValue("tweet");
                    break;
                case EmbedType.Html:
                    writer.WriteStringValue("html");
                    break;
                default:
                    throw new JsonException("Invalid embed type");
            }
        }
    }
}
