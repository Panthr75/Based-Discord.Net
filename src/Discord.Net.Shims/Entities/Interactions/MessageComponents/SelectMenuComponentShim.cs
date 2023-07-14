using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="SelectMenuComponent"/>
    /// </summary>
    public class SelectMenuComponentShim : IMessageComponentShim, IConvertibleShim<SelectMenuComponent>, IEquatable<SelectMenuComponentShim>, IEquatable<SelectMenuComponent>
    {
        /// <inheritdoc/>
        public ComponentType Type { get; }

        /// <inheritdoc/>
        public string CustomId { get; }

        /// <summary>
        ///     Gets the menus options to select from.
        /// </summary>
        public IReadOnlyCollection<SelectMenuOption> Options { get; }

        /// <summary>
        ///     Gets the custom placeholder text if nothing is selected.
        /// </summary>
        public string? Placeholder { get; }

        /// <summary>
        ///     Gets the minimum number of items that must be chosen.
        /// </summary>
        public int MinValues { get; }

        /// <summary>
        ///     Gets the maximum number of items that can be chosen.
        /// </summary>
        public int MaxValues { get; }

        /// <summary>
        ///     Gets whether this menu is disabled or not.
        /// </summary>
        public bool IsDisabled { get; }

        /// <summary>
        ///     Gets the allowed channel types for this modal
        /// </summary>
        public IReadOnlyCollection<ChannelType> ChannelTypes { get; }
    }
}
