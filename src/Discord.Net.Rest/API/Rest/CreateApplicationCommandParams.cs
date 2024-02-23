using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Discord.API.Rest
{
    internal class CreateApplicationCommandParams
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public ApplicationCommandType Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("options")]
        public Optional<ApplicationCommandOption[]> Options { get; set; }

        [JsonPropertyName("default_permission")]
        public Optional<bool> DefaultPermission { get; set; }

        [JsonPropertyName("name_localizations")]
        public Optional<Dictionary<string, string>> NameLocalizations { get; set; }

        [JsonPropertyName("description_localizations")]
        public Optional<Dictionary<string, string>> DescriptionLocalizations { get; set; }

        [JsonPropertyName("dm_permission")]
        public Optional<bool> DmPermission { get; set; }

        [JsonPropertyName("default_member_permissions")]
        public Optional<GuildPermission> DefaultMemberPermission { get; set; }

        [JsonPropertyName("nsfw")]
        public Optional<bool> Nsfw { get; set; }

        public CreateApplicationCommandParams()
        {
            Name = string.Empty;
            Description = string.Empty;
        }
        public CreateApplicationCommandParams(string name,
            string description,
            ApplicationCommandType type,
            ApplicationCommandOption[]? options = null,
            IDictionary<string, string>? nameLocalizations = null,
            IDictionary<string, string>? descriptionLocalizations = null,
            bool nsfw = false)
        {
            Name = name;
            Description = description;
            Options = Optional.CreateFromNullable(options);
            Type = type;
            NameLocalizations = Optional.CreateFromNullable(nameLocalizations?.ToDictionary(x => x.Key, x => x.Value));
            DescriptionLocalizations = Optional.CreateFromNullable(descriptionLocalizations?.ToDictionary(x => x.Key, x => x.Value));
            Nsfw = nsfw;
        }
    }
}
