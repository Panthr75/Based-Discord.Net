using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.IO;
using Model = Discord.API.Image;

namespace Discord.Net.Converters
{
    internal class ImageConverter : JsonConverter<Model>
    {
        public override Model Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new InvalidOperationException();
        }

        public override void Write(Utf8JsonWriter writer, Model value, JsonSerializerOptions options)
        {
            if (value.Stream != null)
            {
                byte[] bytes;
                int length;
                if (value.Stream.CanSeek)
                {
                    bytes = new byte[value.Stream.Length - value.Stream.Position];
                    length = value.Stream.Read(bytes, 0, bytes.Length);
                }
                else
                {
                    using (var cloneStream = new MemoryStream())
                    {
                        value.Stream.CopyTo(cloneStream);
                        bytes = new byte[cloneStream.Length];
                        cloneStream.Position = 0;
                        cloneStream.Read(bytes, 0, bytes.Length);
                        length = (int)cloneStream.Length;
                    }
                }

                string base64 = Convert.ToBase64String(bytes, 0, length);
                writer.WriteStringValue($"data:image/jpeg;base64,{base64}");
            }
            else if (value.Hash != null)
                writer.WriteStringValue(value.Hash);
            else
                writer.WriteNullValue();
        }
    }
}
