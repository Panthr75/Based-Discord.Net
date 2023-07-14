using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IThreadChannel"/>
    /// </summary>
    public interface IThreadChannelShim : IThreadChannel, ITextChannelShim
    {
        /// <inheritdoc cref="IThreadChannel.IsArchived"/>
        new bool IsArchived { get; set; }

        /// <inheritdoc cref="IThreadChannel.AutoArchiveDuration"/>
        new ThreadArchiveDuration AutoArchiveDuration { get; set; }

        /// <inheritdoc cref="IThreadChannel.ArchiveTimestamp"/>
        new DateTimeOffset ArchiveTimestamp { get; set; }

        /// <inheritdoc cref="IThreadChannel.IsLocked"/>
        new bool IsLocked { get; set; }

        /// <inheritdoc cref="IThreadChannel.IsInvitable"/>
        new bool? IsInvitable { get; set; }

        /// <inheritdoc cref="IThreadChannel.AppliedTags"/>
        new ICollection<ulong> AppliedTags { get; }

        /// <inheritdoc cref="IThreadChannel.AddUserAsync(IGuildUser, RequestOptions?)"/>
        Task AddUserAsync(IGuildUserShim user, RequestOptions? options = null);

        /// <inheritdoc cref="IThreadChannel.RemoveUserAsync(IGuildUser, RequestOptions?)"/>
        Task RemoveUserAsync(IGuildUserShim user, RequestOptions? options = null);

    }
}
