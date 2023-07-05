using Discord.Utils;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Model = Discord.API.ApplicationCommandOption;

namespace Discord.WebSocket
{
    /// <summary>
    ///     Represents an option for a <see cref="SocketApplicationCommand"/>.
    /// </summary>
    public class SocketApplicationCommandOption : IApplicationCommandOption
    {
        /// <inheritdoc/>
        public string Name { get; private set; }

        /// <inheritdoc/>
        public ApplicationCommandOptionType Type { get; private set; }

        /// <inheritdoc/>
        public string Description { get; private set; }

        /// <inheritdoc/>
        public bool? IsDefault { get; private set; }

        /// <inheritdoc/>
        public bool? IsRequired { get; private set; }

        public bool? IsAutocomplete { get; private set; }

        /// <inheritdoc/>
        public NumericValue? MinValue { get; private set; }

        /// <inheritdoc/>
        public NumericValue? MaxValue { get; private set; }

        /// <inheritdoc/>
        public int? MinLength { get; private set; }

        /// <inheritdoc/>
        public int? MaxLength { get; private set; }

        /// <summary>
        ///     Gets a collection of choices for the user to pick from.
        /// </summary>
        public IReadOnlyCollection<SocketApplicationCommandChoice> Choices { get; private set; }

        /// <summary>
        ///     Gets a collection of nested options.
        /// </summary>
        public IReadOnlyCollection<SocketApplicationCommandOption> Options { get; private set; }

        /// <summary>
        ///     Gets the allowed channel types for this option.
        /// </summary>
        public IReadOnlyCollection<ChannelType> ChannelTypes { get; private set; }

        /// <summary>
        ///     Gets the localization dictionary for the name field of this command option.
        /// </summary>
        public IReadOnlyDictionary<string, string> NameLocalizations { get; private set; }

        /// <summary>
        ///     Gets the localization dictionary for the description field of this command option.
        /// </summary>
        public IReadOnlyDictionary<string, string> DescriptionLocalizations { get; private set; }

        /// <summary>
        ///     Gets the localized name of this command option.
        /// </summary>
        /// <remarks>
        ///     Only returned when the `withLocalizations` query parameter is set to <see langword="false"/> when requesting the command.
        /// </remarks>
        public string? NameLocalized { get; private set; }

        /// <summary>
        ///     Gets the localized description of this command option.
        /// </summary>
        /// <remarks>
        ///     Only returned when the `withLocalizations` query parameter is set to <see langword="false"/> when requesting the command.
        /// </remarks>
        public string? DescriptionLocalized { get; private set; }

        internal SocketApplicationCommandOption()
        {
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.Choices = ImmutableArray<SocketApplicationCommandChoice>.Empty;
            this.Options = ImmutableArray<SocketApplicationCommandOption>.Empty;
            this.ChannelTypes = ImmutableArray<ChannelType>.Empty;
            this.NameLocalizations = ImmutableDictionary<string, string>.Empty;
            this.DescriptionLocalizations = ImmutableDictionary<string, string>.Empty;
        }
        internal static SocketApplicationCommandOption Create(Model model)
        {
            var entity = new SocketApplicationCommandOption();
            entity.Update(model);
            return entity;
        }

        internal void Update(Model model)
        {
            Name = model.Name;
            Type = model.Type;
            Description = model.Description;

            IsDefault = model.Default.ToNullable();

            IsRequired = model.Required.ToNullable();

            MinValue = model.MinValue.ToNullable();

            MaxValue = model.MaxValue.ToNullable();

            IsAutocomplete = model.Autocomplete.ToNullable();

            MinLength = model.MinLength.ToNullable();
            MaxLength = model.MaxLength.ToNullable();

            Choices = model.Choices.IsSpecified
                ? model.Choices.Value.Select(SocketApplicationCommandChoice.Create).ToImmutableArray()
                : ImmutableArray.Create<SocketApplicationCommandChoice>();

            Options = model.Options.IsSpecified
                ? model.Options.Value.Select(Create).ToImmutableArray()
                : ImmutableArray.Create<SocketApplicationCommandOption>();

            ChannelTypes = model.ChannelTypes.IsSpecified
                ? model.ChannelTypes.Value.ToImmutableArray()
                : ImmutableArray.Create<ChannelType>();

            NameLocalizations = model.NameLocalizations.GetValueOrDefault(null)?.ToImmutableDictionary() ??
                                ImmutableDictionary<string, string>.Empty;

            DescriptionLocalizations = model.DescriptionLocalizations.GetValueOrDefault(null)?.ToImmutableDictionary() ??
                                       ImmutableDictionary<string, string>.Empty;

            NameLocalized = model.NameLocalized.GetValueOrDefault();
            DescriptionLocalized = model.DescriptionLocalized.GetValueOrDefault();
        }

        IReadOnlyCollection<IApplicationCommandOptionChoice> IApplicationCommandOption.Choices => Choices;
        IReadOnlyCollection<IApplicationCommandOption> IApplicationCommandOption.Options => Options;
    }
}
