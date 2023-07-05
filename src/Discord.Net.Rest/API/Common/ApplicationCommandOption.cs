using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using Discord.Utils;

namespace Discord.API
{
    internal class ApplicationCommandOption
    {
        [JsonPropertyName("type")]
        public ApplicationCommandOptionType Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("default")]
        public Optional<bool> Default { get; set; }

        [JsonPropertyName("required")]
        public Optional<bool> Required { get; set; }

        [JsonPropertyName("choices")]
        public Optional<ApplicationCommandOptionChoice[]> Choices { get; set; }

        [JsonPropertyName("options")]
        public Optional<ApplicationCommandOption[]> Options { get; set; }

        [JsonPropertyName("autocomplete")]
        public Optional<bool> Autocomplete { get; set; }

        [JsonPropertyName("min_value")]
        public Optional<NumericValue> MinValue { get; set; }

        [JsonPropertyName("max_value")]
        public Optional<NumericValue> MaxValue { get; set; }

        [JsonPropertyName("channel_types")]
        public Optional<ChannelType[]> ChannelTypes { get; set; }

        [JsonPropertyName("name_localizations")]
        public Optional<Dictionary<string, string>> NameLocalizations { get; set; }

        [JsonPropertyName("description_localizations")]
        public Optional<Dictionary<string, string>> DescriptionLocalizations { get; set; }

        [JsonPropertyName("name_localized")]
        public Optional<string> NameLocalized { get; set; }

        [JsonPropertyName("description_localized")]
        public Optional<string> DescriptionLocalized { get; set; }

        [JsonPropertyName("min_length")]
        public Optional<int> MinLength { get; set; }

        [JsonPropertyName("max_length")]
        public Optional<int> MaxLength { get; set; }

        public ApplicationCommandOption() { }

        public ApplicationCommandOption(IApplicationCommandOption cmd)
        {
            Choices = cmd.Choices.Select(x => new ApplicationCommandOptionChoice
            {
                Name = x.Name,
                Value = x.Value
            }).ToArray();

            Options = cmd.Options.Select(x => new ApplicationCommandOption(x)).ToArray();

            ChannelTypes = cmd.ChannelTypes.ToArray();

            Required = cmd.IsRequired ?? Optional<bool>.Unspecified;
            Default = cmd.IsDefault ?? Optional<bool>.Unspecified;
            MinValue = cmd.MinValue ?? Optional<NumericValue>.Unspecified;
            MaxValue = cmd.MaxValue ?? Optional<NumericValue>.Unspecified;
            MinLength = cmd.MinLength ?? Optional<int>.Unspecified;
            MaxLength = cmd.MaxLength ?? Optional<int>.Unspecified;
            Autocomplete = cmd.IsAutocomplete ?? Optional<bool>.Unspecified;

            Name = cmd.Name;
            Type = cmd.Type;
            Description = cmd.Description;

            NameLocalizations = cmd.NameLocalizations?.ToDictionary() ?? Optional<Dictionary<string, string>>.Unspecified;
            DescriptionLocalizations = cmd.DescriptionLocalizations?.ToDictionary() ?? Optional<Dictionary<string, string>>.Unspecified;
            NameLocalized = Optional.CreateFromNullable(cmd.NameLocalized);
            DescriptionLocalized = Optional.CreateFromNullable(cmd.DescriptionLocalized);
        }
        public ApplicationCommandOption(ApplicationCommandOptionProperties option)
        {
            Choices = option.Choices?.Select(x => new ApplicationCommandOptionChoice
            {
                Name = x.Name,
                Value = x.Value,
                NameLocalizations = x.NameLocalizations?.ToDictionary() ?? Optional<Dictionary<string, string>>.Unspecified,
            }).ToArray() ?? Optional<ApplicationCommandOptionChoice[]>.Unspecified;

            Options = option.Options?.Select(x => new ApplicationCommandOption(x)).ToArray() ?? Optional<ApplicationCommandOption[]>.Unspecified;

            Required = option.IsRequired ?? Optional<bool>.Unspecified;

            Default = option.IsDefault ?? Optional<bool>.Unspecified;
            MinValue = option.MinValue ?? Optional<NumericValue>.Unspecified;
            MaxValue = option.MaxValue ?? Optional<NumericValue>.Unspecified;
            MinLength = option.MinLength ?? Optional<int>.Unspecified;
            MaxLength = option.MaxLength ?? Optional<int>.Unspecified;

            ChannelTypes = option.ChannelTypes?.ToArray() ?? Optional<ChannelType[]>.Unspecified;

            Name = option.Name;
            Type = option.Type;
            Description = option.Description;
            Autocomplete = option.IsAutocomplete;

            NameLocalizations = option.NameLocalizations?.ToDictionary() ?? Optional<Dictionary<string, string>>.Unspecified;
            DescriptionLocalizations = option.DescriptionLocalizations?.ToDictionary() ?? Optional<Dictionary<string, string>>.Unspecified;
        }
    }
}
