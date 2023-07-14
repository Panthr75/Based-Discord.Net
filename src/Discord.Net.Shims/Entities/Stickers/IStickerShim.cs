using System.Collections.Generic;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="ISticker"/>
    /// </summary>
    public interface IStickerShim : ISticker, IStickerItemShim
    {
        /// <inheritdoc cref="ISticker.Id"/>
        new ulong Id { get; set; }
        /// <inheritdoc cref="ISticker.PackId"/>
        new ulong PackId { get; set; }
        /// <inheritdoc cref="ISticker.Name"/>
        new string Name { get; set; }
        /// <inheritdoc cref="ISticker.Description"/>
        new string? Description { get; set; }
        /// <inheritdoc cref="ISticker.Tags"/>
        new ICollection<string> Tags { get; }
        /// <inheritdoc cref="ISticker.Format"/>
        new StickerFormatType Format { get; set; }
        /// <inheritdoc cref="ISticker.IsAvailable"/>
        new bool? IsAvailable { get; set; }
        /// <inheritdoc cref="ISticker.SortOrder"/>
        new int? SortOrder { get; set; }
    }
}
