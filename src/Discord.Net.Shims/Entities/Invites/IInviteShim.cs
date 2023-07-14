using System;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IInvite"/>
    /// </summary>
    public interface IInviteShim : IInvite, IShimEntity<string>
    {
        /// <inheritdoc cref="IInvite.Code"/>
        new string Code { get; set; }

        /// <inheritdoc cref="IInvite.Inviter"/>
        new IUserShim? Inviter { get; set; }

        /// <inheritdoc cref="IInvite.Channel"/>
        new IChannelShim Channel { get; set; }

        /// <inheritdoc cref="IInvite.Guild"/>
        new IGuildShim Guild { get; }

        /// <inheritdoc cref="IInvite.PresenceCount"/>
        new int? PresenceCount { get; set; }
        /// <inheritdoc cref="IInvite.MemberCount"/>
        new int? MemberCount { get; set; }

        /// <inheritdoc cref="IInvite.TargetUser"/>
        new IUserShim? TargetUser { get; set; }
        /// <inheritdoc cref="IInvite.TargetUserType"/>
        new TargetUserType TargetUserType { get; set; }

        /// <inheritdoc cref="IInvite.Application"/>
        new IApplicationShim? Application { get; set; }

        /// <inheritdoc cref="IInvite.ExpiresAt"/>
        new DateTimeOffset? ExpiresAt { get; set; }
    }
}
