using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

namespace Discord.API
{
    internal class SelectMenuComponent : IMessageComponent
    {
        [JsonPropertyName("type")]
        public ComponentType Type { get; set; }

        [JsonPropertyName("custom_id")]
        public string CustomId { get; set; }

        [JsonPropertyName("options")]
        public Optional<SelectMenuOption[]> Options { get; set; }

        [JsonPropertyName("placeholder")]
        public Optional<string> Placeholder { get; set; }

        [JsonPropertyName("min_values")]
        public int MinValues { get; set; }

        [JsonPropertyName("max_values")]
        public int MaxValues { get; set; }

        [JsonPropertyName("disabled")]
        public bool Disabled { get; set; }

        [JsonPropertyName("channel_types")]
        public Optional<ChannelType[]> ChannelTypes { get; set; }

        [JsonPropertyName("resolved")]
        public Optional<MessageComponentInteractionDataResolved> Resolved { get; set; }

        [JsonPropertyName("values")]
        public Optional<string[]> Values { get; set; }
        public SelectMenuComponent()
        {
            CustomId = string.Empty;
        }

        public SelectMenuComponent(Discord.SelectMenuComponent component)
        {
            Type = component.Type;
            CustomId = component.CustomId;
            Options = Optional.CreateFromNullable(component.Options?.Select(x => new SelectMenuOption(x))?.ToArray());
            Placeholder = Optional.CreateFromNullable(component.Placeholder);
            MinValues = component.MinValues;
            MaxValues = component.MaxValues;
            Disabled = component.IsDisabled;
            ChannelTypes = component.ChannelTypes.ToArray();
        }
    }
}
