using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IGuildUser"/>
    /// </summary>
    public interface IGuildUserShim : IGuildUser, IUserShim, IVoiceStateShim
    {
        /// <inheritdoc cref="IGuildUser.JoinedAt"/>
        new DateTimeOffset? JoinedAt { get; set; }
        /// <inheritdoc cref="IGuildUser.DisplayName"/>
        new string DisplayName { get; }
        /// <inheritdoc cref="IGuildUser.Nickname"/>
        new string? Nickname { get; set; }
        /// <inheritdoc cref="IGuildUser.DisplayAvatarId"/>
        new string? DisplayAvatarId { get; }
        /// <inheritdoc cref="IGuildUser.GuildAvatarId"/>
        new string? GuildAvatarId { get; set; }
        /// <inheritdoc cref="IGuildUser.GuildPermissions"/>
        new GuildPermissions GuildPermissions { get; }
        /// <inheritdoc cref="IGuildUser.Guild"/>
        new IGuildShim Guild { get; }
        /// <inheritdoc cref="IGuildUser.GuildId"/>
        new ulong GuildId { get; }
        /// <inheritdoc cref="IGuildUser.PremiumSince"/>
        new DateTimeOffset? PremiumSince { get; set; }

        /// <inheritdoc cref="IGuildUser.IsPending"/>
        new bool? IsPending { get; set; }

        /// <inheritdoc cref="IGuildUser.TimedOutUntil"/>
        new DateTimeOffset? TimedOutUntil { get; set; }

        /// <inheritdoc cref="IGuildUser.Flags"/>
        new GuildUserFlags Flags { get; set; }
    }
}
