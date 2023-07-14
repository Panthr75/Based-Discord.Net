using System;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IInviteMetadata"/>
    /// </summary>
    public interface IInviteMetadataShim : IInviteMetadata, IInviteShim
    {
        /// <inheritdoc cref="IInviteMetadata.IsTemporary"/>
        new bool IsTemporary { get; set; }
        /// <inheritdoc cref="IInviteMetadata.MaxAge"/>
        new int? MaxAge { get; set; }
        /// <inheritdoc cref="IInviteMetadata.MaxUses"/>
        new int? MaxUses { get; set; }
        /// <inheritdoc cref="IInviteMetadata.Uses"/>
        new int? Uses { get; set; }
        /// <inheritdoc cref="IInviteMetadata.CreatedAt"/>
        new DateTimeOffset? CreatedAt { get; set; }
    }
}
