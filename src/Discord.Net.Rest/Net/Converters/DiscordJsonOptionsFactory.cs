using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Discord.Net.Converters;

internal static class DiscordJsonOptionsFactory
{
    private static readonly EmbedTypeConverter embedTypeConverter = new();
    private static readonly UInt64Converter uint64Converter = new();
    private static readonly GuildFeaturesConverter guildFeaturesConverter = new();
    private static readonly ApplicationCommandOptionValueConverter applicationCommandOptionValueConverter = new();
    private static readonly EnumStringValueConverter<GuildPermission> guildPermissionConverter = new();
    private static readonly ImageConverter imageConverter = new();
    private static readonly InteractionConverter interactionConverter = new();
    private static readonly NumericValueConverter numericValueConverter = new();
    private static readonly JsonNodeConverter jsonNodeConverter = new();
    private static readonly MessageComponentConverter messageComponentConverter = new();
    private static readonly StringEntityConverter stringEntityConverter = new();
    private static readonly UInt64EntityConverter uint64EntityConverter = new();
    private static readonly UInt64EntityOrIdConverter uint64EntityOrIdConverter = new();
    private static readonly UserStatusConverter userStatusConverter = new();
    private static readonly SelectMenuDefaultValueTypeConverter selectMenuDefaultValueTypeConverter = new();

    public static JsonSerializerOptions CreateDefaultOptions()
    {
        var result = new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            IncludeFields = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            {
                Modifiers = { Optional.JsonModifier }
            }
        };

        AddDefaultConverters(result);
        return result;
    }

    private static void AddDefaultConverters(JsonSerializerOptions options)
    {
        options.Converters.Add(embedTypeConverter);
        options.Converters.Add(uint64Converter);
        options.Converters.Add(guildFeaturesConverter);
        options.Converters.Add(applicationCommandOptionValueConverter);
        options.Converters.Add(guildPermissionConverter);
        options.Converters.Add(imageConverter);
        options.Converters.Add(interactionConverter);
        options.Converters.Add(numericValueConverter);
        options.Converters.Add(jsonNodeConverter);
        options.Converters.Add(messageComponentConverter);
        options.Converters.Add(stringEntityConverter);
        options.Converters.Add(uint64EntityConverter);
        options.Converters.Add(uint64EntityOrIdConverter);
        options.Converters.Add(userStatusConverter);
        options.Converters.Add(selectMenuDefaultValueTypeConverter);
    }
}
