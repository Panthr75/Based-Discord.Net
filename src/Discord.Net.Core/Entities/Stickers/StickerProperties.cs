using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Discord
{
    /// <summary>
    ///     Represents a class used to modify stickers.
    /// </summary>
    public class StickerProperties
    {
        private Optional<string> _name;
        private Optional<string> _description;
        private Optional<IEnumerable<string>> _tags;

        /// <summary>
        ///     Gets or sets the name of the sticker.
        /// </summary>
        public Optional<string> Name
        {
            get => this._name;
            set => this._name = value.Map(name => StickerProperties.ValidateName(name, nameof(value)));
        }

        /// <summary>
        ///     Gets or sets the description of the sticker.
        /// </summary>
        public Optional<string> Description
        {
            get => this._description;
            set => this._description = value.Map(description => StickerProperties.ValidateDescription(description, nameof(value)));
        }

        /// <summary>
        ///     Gets or sets the tags of the sticker.
        /// </summary>
        public Optional<IEnumerable<string>> Tags
        {
            get => this._tags;
            // convert to immutable array to prevent modification
            set => this._tags = value.Map(tags => (IEnumerable<string>)ValidateTags(tags, nameof(value)).ToImmutableArray());
        }

        internal static string ValidateName(string? name, string? paramName = null)
        {
            Preconditions.NotNull(name, nameof(name));
            name = name.Trim();

            Preconditions.AtLeast(name.Length, 2, paramName ?? nameof(name), "Length of sticker name must be at least 2 characters.");

            Preconditions.AtMost(name.Length, 30, paramName ?? nameof(name), "Length of sticker name cannot exceed 30 characters.");

            return name;
        }
        internal static string ValidateDescription(string? description, string? paramName = null)
        {
            if (description is null)
            {
                return string.Empty;
            }
            description = description.Trim();
            if (description.Length == 0)
            {
                return description;
            }

            Preconditions.AtLeast(description.Length, 2, paramName ?? nameof(description), "Length of sticker description must be at least 2 characters, or 0 to specify no description..");

            Preconditions.AtMost(description.Length, 100, paramName ?? nameof(description), "Length of sticker description cannot exceed 100 characters.");

            return description;
        }
        internal static IEnumerable<string> ValidateTags(IEnumerable<string?> tags, string? paramName = null)
        {
            IEnumerable<string> result = tags.Where(t => t is not null)!;
            var tagString = string.Join(", ", result);

            StickerProperties.ValidateTagString(tagString, paramName);

            return result;
        }
        internal static string ValidateTagString(IEnumerable<string?> tags, string? paramName = null)
        {
            IEnumerable<string> result = tags.Where(t => t is not null)!;

            return StickerProperties.ValidateTagString(string.Join(", ", result), paramName);
        }
        internal static string ValidateTagString(string tags, string? paramName = null)
        {
            Preconditions.AtLeast(tags.Length, 1, paramName ?? nameof(tags));
            Preconditions.AtMost(tags.Length, 200, paramName ?? nameof(tags));

            return tags;
        }
    }
}
