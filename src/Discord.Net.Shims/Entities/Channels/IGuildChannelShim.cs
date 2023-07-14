using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IGuildChannel"/>
    /// </summary>
    public interface IGuildChannelShim : IGuildChannel, IChannelShim
    {
        ///<inheritdoc cref="IGuildChannel.Position"/>
        new int Position { get; set; }

        ///<inheritdoc cref="IGuildChannel.Flags"/>
        new ChannelFlags Flags { get; set; }

        ///<inheritdoc cref="IGuildChannel.Guild"/>
        new IGuildShim Guild { get; }

        ///<inheritdoc cref="IGuildChannel.PermissionOverwrites"/>
        new ICollection<Overwrite> PermissionOverwrites { get; }
    }
}
