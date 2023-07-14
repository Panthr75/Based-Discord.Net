namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="ICustomSticker"/>
    /// </summary>
    public interface ICustomStickerShim : IStickerShim, ICustomSticker
    {
        /// <inheritdoc cref="ICustomSticker.AuthorId"/>
        new ulong? AuthorId { get; set; }

        /// <inheritdoc cref="ICustomSticker.Guild"/>
        new IGuildShim? Guild { get; }

    }
}
