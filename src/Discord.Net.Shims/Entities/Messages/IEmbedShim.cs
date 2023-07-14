using System;
using System.Collections.Generic;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IEmbed"/>
    /// </summary>
    public interface IEmbedShim : IEmbed
    {
        /// <inheritdoc cref="IEmbed.Url"/>
        new string? Url { get; set; }
        /// <inheritdoc cref="IEmbed.Title"/>
        new string? Title { get; set; }
        /// <inheritdoc cref="IEmbed.Description"/>
        new string? Description { get; set; }
        /// <inheritdoc cref="IEmbed.Type"/>
        new EmbedType Type { get; set; }
        /// <inheritdoc cref="IEmbed.Timestamp"/>
        new DateTimeOffset? Timestamp { get; set; }
        /// <inheritdoc cref="IEmbed.Color"/>
        new Color? Color { get; set; }
        /// <inheritdoc cref="IEmbed.Image"/>
        new EmbedImageShim? Image { get; set; }
        /// <inheritdoc cref="IEmbed.Video"/>
        new EmbedVideoShim? Video { get; set; }
        /// <inheritdoc cref="IEmbed.Author"/>
        new EmbedAuthorShim? Author { get; set; }
        /// <inheritdoc cref="IEmbed.Footer"/>
        new EmbedFooterShim? Footer { get; set; }
        /// <inheritdoc cref="IEmbed.Provider"/>
        new EmbedProviderShim? Provider { get; set; }
        /// <inheritdoc cref="IEmbed.Thumbnail"/>
        new EmbedThumbnailShim? Thumbnail { get; set; }
        /// <inheritdoc cref="IEmbed.Fields"/>
        new ICollection<EmbedFieldShim> Fields { get; }
    }
}
