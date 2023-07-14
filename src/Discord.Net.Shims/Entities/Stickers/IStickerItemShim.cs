namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IStickerItem"/>
    /// </summary>
    public interface IStickerItemShim : IStickerItem
    {
        /// <inheritdoc cref="IStickerItem.Id"/>
        new ulong Id { get; }
        /// <inheritdoc cref="IStickerItem.Name"/>
        new string Name { get; }
        /// <inheritdoc cref="IStickerItem.Format"/>
        new StickerFormatType Format { get; }
    }
}
