using System.Text.Json;
using System.Text.Json.Serialization;
using System;

namespace Discord.Net.Converters
{
    internal class UserStatusConverter : JsonConverter<UserStatus>
    {
        public override UserStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString() switch
            {
                "online" => UserStatus.Online,
                "idle" => UserStatus.Idle,
                "dnd" => UserStatus.DoNotDisturb,
                "invisible" => UserStatus.Invisible,//Should never happen
                "offline" => UserStatus.Offline,
                _ => throw new JsonException("Unknown user status"),
            };
        }

        public override void Write(Utf8JsonWriter writer, UserStatus value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value switch
            {
                UserStatus.Online => "online",
                UserStatus.Idle or UserStatus.AFK => "idle",
                UserStatus.DoNotDisturb => "dnd",
                UserStatus.Invisible => "invisible",
                UserStatus.Offline => "offline",
                _ => throw new JsonException("Invalid user status")
            });
        }
    }
}
