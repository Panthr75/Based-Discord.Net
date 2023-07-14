namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IAttachment"/>
    /// </summary>
    public interface IAttachmentShim : IAttachment
    {
        /// <inheritdoc cref="IAttachment.Filename"/>
        new string Filename { get; set; }
        /// <inheritdoc cref="IAttachment.Url"/>
        new string Url { get; set; }
        /// <inheritdoc cref="IAttachment.ProxyUrl"/>
        new string ProxyUrl { get; set; }
        /// <inheritdoc cref="IAttachment.Size"/>
        new int Size { get; set; }
        /// <inheritdoc cref="IAttachment.Height"/>
        new int? Height { get; set; }
        /// <inheritdoc cref="IAttachment.Width"/>
        new int? Width { get; set; }
        /// <inheritdoc cref="IAttachment.Ephemeral"/>
        new bool Ephemeral { get; set; }
        /// <inheritdoc cref="IAttachment.Description"/>
        new string? Description { get; set; }
        /// <inheritdoc cref="IAttachment.ContentType"/>
        new string? ContentType { get; set; }

        /// <inheritdoc cref="IAttachment.Duration"/>
        new double? Duration { get; set; }

        /// <inheritdoc cref="IAttachment.Waveform"/>
        new string? Waveform { get; set; }
    }
}
