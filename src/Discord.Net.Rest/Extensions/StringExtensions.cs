using Discord.Net.Converters;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Linq;
using System.Text.Json.Serialization.Metadata;

namespace Discord.Rest
{
    /// <summary>
    ///     Responsible for formatting certain entities as Json <see langword="string"/>, to reuse later on.
    /// </summary>
    public static class StringExtensions
    {
        private static Lazy<JsonSerializerOptions> _settings = new(() =>
        {
            JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
            {
                AllowTrailingCommas = true,
                IncludeFields = true,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
                {
                    Modifiers = { Optional.OptionalModifier }
                }
            };
            serializerOptions.AddConverter<EmbedTypeConverter>();
            return serializerOptions;
        });

        /// <summary>
        ///     Gets a Json formatted <see langword="string"/> from an <see cref="EmbedBuilder"/>.
        /// </summary>
        /// <remarks>
        ///     See <see cref="EmbedBuilderUtils.TryParse(string, out EmbedBuilder)"/> to parse Json back into embed.
        /// </remarks>
        /// <param name="builder">The builder to format as Json <see langword="string"/>.</param>
        /// <returns>A Json <see langword="string"/> containing the data from the <paramref name="builder"/>.</returns>
        public static string ToJsonString(this EmbedBuilder builder)
            => ToJsonString(builder.Build());

        /// <summary>
        ///     Gets a Json formatted <see langword="string"/> from an <see cref="Embed"/>.
        /// </summary>
        /// <remarks>
        ///     See <see cref="EmbedBuilderUtils.TryParse(string, out EmbedBuilder)"/> to parse Json back into embed.
        /// </remarks>
        /// <param name="embed">The embed to format as Json <see langword="string"/>.</param>
        /// <returns>A Json <see langword="string"/> containing the data from the <paramref name="embed"/>.</returns>
        public static string ToJsonString(this Embed embed)
            => JsonSerializer.Serialize(embed.ToModel(), _settings.Value);
    }
}
