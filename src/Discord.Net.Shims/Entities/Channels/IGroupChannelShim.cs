using System.Collections.Generic;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IGroupChannel"/>
    /// </summary>
    public interface IGroupChannelShim : IGroupChannel, IMessageChannelShim, IPrivateChannelShim, IAudioChannelShim
    {
        /// <inheritdoc cref="IPrivateChannelShim.Recipients"/>
        new ICollection<IUserShim> Recipients { get; }
    }
}
