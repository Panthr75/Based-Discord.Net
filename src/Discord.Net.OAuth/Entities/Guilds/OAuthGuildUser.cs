using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Discord.OAuth;

/// <summary>
/// A representation of an OAuth Guild User.
/// </summary>
public abstract class OAuthGuildUser : OAuthUser, IGuildUser
{
    internal OAuthGuildUser(OAuthGuild guild, OAuthGlobalUser globalUser)
        : base(globalUser.Id)
    {
        this.Guild = guild;
        this.GlobalUser = globalUser;
    }

    internal sealed override OAuthGlobalUser GlobalUser { get; set; }

    /// <inheritdoc cref="IGuildUser.Guild"/>
    public OAuthGuild Guild { get; }

    /// <inheritdoc/>
    public sealed override bool IsWebhook => false;

    /// <summary>
    /// Gets whether or not this guild member is the guild
    /// owner.
    /// </summary>
    public abstract bool IsOwner { get; }

    /// <inheritdoc/>
    public string DisplayName => this.Nickname.GetValueOrDefault(null) ?? this.GlobalName ?? this.Username;
    /// <inheritdoc/>
    public string? DisplayAvatarId => this.GuildAvatarId.GetValueOrDefault(null) ?? this.AvatarId;
    /// <inheritdoc/>
    public GuildPermissions GuildPermissions { get; internal set; }
    /// <inheritdoc/>
    public ulong GuildId => this.Guild.Id;
    /// <inheritdoc cref="IGuildUser.Nickname"/>
    public abstract Optional<string?> Nickname { get; }
    /// <inheritdoc cref="IGuildUser.GuildAvatarId"/>
    public abstract Optional<string?> GuildAvatarId { get; }
    /// <inheritdoc cref="IGuildUser.IsPending"/>
    public abstract Optional<bool?> IsPending { get; }
    /// <inheritdoc cref="IGuildUser.JoinedAt"/>
    public abstract Optional<DateTimeOffset?> JoinedAt { get; }
    /// <inheritdoc cref="IGuildUser.Flags"/>
    public abstract Optional<GuildUserFlags> MemberFlags { get; }
    /// <inheritdoc cref="IGuildUser.RoleIds"/>
    public abstract Optional<IReadOnlyCollection<ulong>> RoleIds { get; }
    /// <summary>
    /// The roles of the user.
    /// </summary>
    public abstract Optional<IReadOnlyCollection<IRole>> Roles { get; }
    /// <inheritdoc cref="IGuildUser.PremiumSince"/>
    public abstract Optional<DateTimeOffset?> PremiumSince { get; }
    /// <inheritdoc cref="IGuildUser.Hierarchy"/>
    public abstract Optional<int> Hierarchy { get; }
    /// <inheritdoc cref="IGuildUser.TimedOutUntil"/>
    public abstract Optional<DateTimeOffset?> TimedOutUntil { get; }
    /// <inheritdoc cref="IVoiceState.IsMuted"/>
    public abstract Optional<bool> IsMuted { get; }
    /// <inheritdoc cref="IVoiceState.IsDeafened"/>
    public abstract Optional<bool> IsDeafened { get; }

    /// <summary>
    /// Gets whether or not this guild user has been fetched
    /// from a client or from OAuth.
    /// </summary>
    public abstract bool Fetched { get; }

    /// <inheritdoc cref="IGuildUser.GetGuildAvatarUrl(ImageFormat, ushort)"/>
    public Optional<string?> GetGuildAvatarUrl(ImageFormat format = ImageFormat.Auto, ushort size = 128)
        => this.GuildAvatarId.Map(x => CDN.GetGuildUserAvatarUrl(this.Id, this.GuildId, x, size, format));

    /// <summary>
    /// Updates this guild user from the specified discord
    /// client.
    /// </summary>
    /// <param name="client">
    /// The client to fetch the guild user from.
    /// </param>
    /// <param name="options">
    /// The options for sending the request.
    /// </param>
    /// <returns>
    /// A task.
    /// </returns>
    public abstract Task UpdateGuildUserFromClientAsync(IDiscordClient client, RequestOptions? options = null);

    /// <summary>
    /// Updates this guild user from the specified discord
    /// client.
    /// </summary>
    /// <param name="client">
    /// The client to fetch the guild user from.
    /// </param>
    /// <param name="options">
    /// The options for sending the request.
    /// </param>
    /// <returns>
    /// A task.
    /// </returns>
    public abstract Task UpdateGuildUserFromClientAsync(DiscordRestClient client, RequestOptions? options = null);

    /// <summary>
    /// Updates this guild user from the specified discord
    /// client.
    /// </summary>
    /// <param name="client">
    /// The client to fetch the guild user from.
    /// </param>
    /// <param name="options">
    /// The options for sending the request.
    /// </param>
    /// <returns>
    /// A task.
    /// </returns>
    public abstract Task UpdateGuildUserFromClientAsync(BaseSocketClient client, RequestOptions? options = null);

    /// <summary>
    /// Updates this guild user from discord oauth.
    /// </summary>
    /// <param name="options">
    /// The options for sending the request.
    /// </param>
    /// <returns>
    /// A task.
    /// </returns>
    public abstract Task UpdateGuildUserFromOAuthAsync(RequestOptions? options = null);

    #region IGuildUser
    /// <inheritdoc/>
    IGuild IGuildUser.Guild => this.Guild;

    /// <inheritdoc/>
    ChannelPermissions IGuildUser.GetPermissions(IGuildChannel channel)
        => ChannelPermissions.None;

    /// <inheritdoc/>
    GuildUserFlags IGuildUser.Flags => this.MemberFlags.GetValueOrDefault(GuildUserFlags.None);

    /// <inheritdoc/>
    string? IGuildUser.Nickname => this.Nickname.GetValueOrDefault(null);
    /// <inheritdoc/>
    string? IGuildUser.GuildAvatarId => this.GuildAvatarId.GetValueOrDefault(null);
    /// <inheritdoc/>
    bool? IGuildUser.IsPending => this.IsPending.GetValueOrDefault(null);
    /// <inheritdoc/>
    DateTimeOffset? IGuildUser.JoinedAt => this.JoinedAt.GetValueOrDefault(null);
    /// <inheritdoc/>
    IReadOnlyCollection<ulong> IGuildUser.RoleIds => this.RoleIds.GetValueOrDefault(ImmutableArray<ulong>.Empty);
    /// <inheritdoc/>
    DateTimeOffset? IGuildUser.PremiumSince => this.PremiumSince.GetValueOrDefault(null);
    /// <inheritdoc/>
    int IGuildUser.Hierarchy => this.Hierarchy.GetValueOrDefault(this.IsOwner ? int.MaxValue : 0);
    /// <inheritdoc/>
    DateTimeOffset? IGuildUser.TimedOutUntil => this.TimedOutUntil.GetValueOrDefault(null);

    /// <inheritdoc/>
    string? IGuildUser.GetGuildAvatarUrl(ImageFormat format, ushort size)
        => this.GetGuildAvatarUrl(format, size).GetValueOrDefault(null);

    /// <inheritdoc/>
    Task IGuildUser.KickAsync(string? reason, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    /// <inheritdoc/>
    Task IGuildUser.ModifyAsync(Action<GuildUserProperties> func, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    /// <inheritdoc/>
    Task IGuildUser.AddRoleAsync(ulong roleId, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    /// <inheritdoc/>
    Task IGuildUser.AddRoleAsync(IRole role, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    /// <inheritdoc/>
    Task IGuildUser.AddRolesAsync(IEnumerable<ulong> roleIds, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    /// <inheritdoc/>
    Task IGuildUser.AddRolesAsync(IEnumerable<IRole> roles, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    /// <inheritdoc/>
    Task IGuildUser.RemoveRoleAsync(ulong roleId, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    /// <inheritdoc/>
    Task IGuildUser.RemoveRoleAsync(IRole role, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    /// <inheritdoc/>
    Task IGuildUser.RemoveRolesAsync(IEnumerable<ulong> roleIds, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    /// <inheritdoc/>
    Task IGuildUser.RemoveRolesAsync(IEnumerable<IRole> roles, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    /// <inheritdoc/>
    Task IGuildUser.SetTimeOutAsync(TimeSpan span, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    /// <inheritdoc/>
    Task IGuildUser.RemoveTimeOutAsync(RequestOptions? options)
        => Task.FromException(new NotSupportedException());
    #endregion

    #region IVoiceState
    /// <inheritdoc/>
    bool IVoiceState.IsMuted => this.IsMuted.GetValueOrDefault(false);
    /// <inheritdoc/>
    bool IVoiceState.IsDeafened => this.IsDeafened.GetValueOrDefault(false);
    /// <inheritdoc/>
    bool IVoiceState.IsSelfDeafened => false;
    /// <inheritdoc/>
    bool IVoiceState.IsSelfMuted => false;
    /// <inheritdoc/>
    bool IVoiceState.IsSuppressed => false;
    /// <inheritdoc/>
    bool IVoiceState.IsStreaming => false;
    /// <inheritdoc/>
    bool IVoiceState.IsVideoing => false;
    /// <inheritdoc/>
    DateTimeOffset? IVoiceState.RequestToSpeakTimestamp => null;
    /// <inheritdoc/>
    IVoiceChannel? IVoiceState.VoiceChannel => null;
    /// <inheritdoc/>
    string? IVoiceState.VoiceSessionId => null;
    #endregion
}