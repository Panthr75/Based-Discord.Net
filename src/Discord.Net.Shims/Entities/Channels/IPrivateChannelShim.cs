using System.Collections.Generic;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IPrivateChannel"/>
    /// </summary>
    public interface IPrivateChannelShim : IPrivateChannel, IChannelShim
    {
        /// <inheritdoc cref="IPrivateChannel.Recipients"/>
        new IReadOnlyCollection<IUserShim> Recipients { get; }
    }
}
