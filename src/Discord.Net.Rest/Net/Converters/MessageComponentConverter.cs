using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Text.Json.Nodes;

namespace Discord.Net.Converters
{
    internal class MessageComponentConverter : JsonConverter<IMessageComponent>
    {
        public override void Write(Utf8JsonWriter writer, IMessageComponent value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }

        public override IMessageComponent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            JsonObject ob = JsonNode.Parse(ref reader, nodeOptions: new JsonNodeOptions()
            {
                PropertyNameCaseInsensitive = options.PropertyNameCaseInsensitive
            })!.AsObject();


            int type = 0;
            if (ob.TryGetPropertyValue("type", out JsonNode? typeValue))
            {
                type = typeValue.Deserialize<int>();
            }

            return (ComponentType)type switch
            {
                ComponentType.ActionRow => ob.Deserialize<API.ActionRowComponent>(options),
                ComponentType.Button => ob.Deserialize<API.ButtonComponent>(options),
                ComponentType.SelectMenu or ComponentType.ChannelSelect or ComponentType.MentionableSelect or ComponentType.RoleSelect or ComponentType.UserSelect => ob.Deserialize<API.SelectMenuComponent>(options),
                ComponentType.TextInput => ob.Deserialize<API.TextInputComponent>(options),
                _ => null,
            };
        }
    }
}
