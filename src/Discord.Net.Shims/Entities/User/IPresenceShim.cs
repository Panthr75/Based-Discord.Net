using System.Collections.Generic;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IPresence"/>
    /// </summary>
    public interface IPresenceShim : IPresence
    {
        /// <inheritdoc cref="IPresence.Status"/>
        new UserStatus Status { get; set; }
        /// <inheritdoc cref="IPresence.ActiveClients"/>
        new ICollection<ClientType> ActiveClients { get; }
        /// <inheritdoc cref="IPresence.Activities"/>
        new ICollection<IActivityShim> Activities { get; }
    }
}
