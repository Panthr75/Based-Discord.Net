using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Model = Discord.API.ThreadMember;

namespace Discord.WebSocket
{
    /// <summary>
    ///     Represents a thread user received over the gateway.
    /// </summary>
    public class SocketThreadUser : SocketUser, IThreadUser, IGuildUser
    {
        /// <summary>
        ///     Gets the <see cref="SocketThreadChannel"/> this user is in.
        /// </summary>
        public SocketThreadChannel Thread { get; private set; }

        /// <inheritdoc/>
        public DateTimeOffset ThreadJoinedAt { get; private set; }

        /// <summary>
        ///     Gets the guild this user is in.
        /// </summary>
        public SocketGuild Guild { get; private set; }

        /// <inheritdoc/>
        public DateTimeOffset? JoinedAt
            => GuildUser?.JoinedAt;

        /// <inheritdoc/>
        public string DisplayName
        {
            get
            {
                if (GuildUser is null)
                {
                    return string.Empty;
                }

                return GuildUser.Nickname ?? GuildUser.GlobalName ?? GuildUser.Username;
            }
        }

        /// <inheritdoc/>
        public string? Nickname
            => GuildUser?.Nickname;

        /// <inheritdoc/>
        public DateTimeOffset? PremiumSince
            => GuildUser?.PremiumSince;

        /// <inheritdoc/>
        public DateTimeOffset? TimedOutUntil
            => GuildUser?.TimedOutUntil;

        /// <inheritdoc/>
        public bool? IsPending
            => GuildUser?.IsPending;
        /// <inheritdoc />
        public int Hierarchy
            => GuildUser?.Hierarchy ?? 0;

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException" accessor="set">
        /// Thrown if <see cref="GuildUser"/> is <see langword="null"/>.
        /// </exception>
        public override string? AvatarId
        {
            get => GuildUser?.AvatarId;
            internal set
            {
                if (GuildUser is null)
                {
                    throw new InvalidOperationException("This thread user doesn't have an associated guild user!");
                }
                GuildUser.AvatarId = value;
            }
        }
        /// <inheritdoc/>
        public string? DisplayAvatarId => GuildAvatarId ?? AvatarId;

        /// <inheritdoc/>
        public string? GuildAvatarId
            => GuildUser?.GuildAvatarId;

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException" accessor="set">
        /// Thrown if <see cref="GuildUser"/> is <see langword="null"/>.
        /// </exception>
        public override ushort DiscriminatorValue
        {
            get => GuildUser?.DiscriminatorValue ?? 0;
            internal set
            {
                if (GuildUser is null)
                {
                    throw new InvalidOperationException("This thread user doesn't have an associated guild user!");
                }

                GuildUser.DiscriminatorValue = value;
            }
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException" accessor="set">
        /// Thrown if <see cref="GuildUser"/> is <see langword="null"/>.
        /// </exception>
        public override bool IsBot
        {
            get => GuildUser?.IsBot ?? false;
            internal set
            {
                if (GuildUser is null)
                {
                    throw new InvalidOperationException("This thread user doesn't have an associated guild user!");
                }

                GuildUser.IsBot = value;
            }
        }

        /// <inheritdoc/>
        public override bool IsWebhook
            => GuildUser?.IsWebhook ?? false;

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException" accessor="set">
        /// Thrown if <see cref="GuildUser"/> is <see langword="null"/>.
        /// </exception>
        public override string Username
        {
            get => GuildUser?.Username ?? string.Empty;
            internal set
            {
                if (GuildUser is null)
                {
                    throw new InvalidOperationException("This thread user doesn't have an associated guild user!");
                }
                GuildUser.Username = value;
            }
        }

        /// <inheritdoc/>
        public bool IsDeafened
            => GuildUser?.IsDeafened ?? false;

        /// <inheritdoc/>
        public bool IsMuted
            => GuildUser?.IsMuted ?? false;

        /// <inheritdoc/>
        public bool IsSelfDeafened
            => GuildUser?.IsSelfDeafened ?? false;

        /// <inheritdoc/>
        public bool IsSelfMuted
            => GuildUser?.IsSelfMuted ?? false;

        /// <inheritdoc/>
        public bool IsSuppressed
            => GuildUser?.IsSuppressed ?? false;

        /// <inheritdoc/>
        public IVoiceChannel? VoiceChannel
            => GuildUser?.VoiceChannel;

        /// <inheritdoc/>
        public string? VoiceSessionId
            => GuildUser?.VoiceSessionId;

        /// <inheritdoc/>
        public bool IsStreaming
            => GuildUser?.IsStreaming ?? false;

        /// <inheritdoc/>
        public bool IsVideoing
            => GuildUser?.IsVideoing ?? false;

        /// <inheritdoc/>
        public GuildUserFlags Flags
            => GuildUser?.Flags ?? GuildUserFlags.None;

        /// <inheritdoc/>
        public DateTimeOffset? RequestToSpeakTimestamp
            => GuildUser?.RequestToSpeakTimestamp;

        /// <inheritdoc cref="IThreadUser.GuildUser"/>
        public SocketGuildUser? GuildUser { get; private set; }

        internal SocketThreadUser(SocketGuild guild, SocketThreadChannel thread, ulong userId, SocketGuildUser? member = null)
            : base(guild.Discord, userId)
        {
            Thread = thread;
            Guild = guild;
            if (member is not null)
                GuildUser = member;
        }

        internal static SocketThreadUser Create(SocketGuild guild, SocketThreadChannel thread, Model model, SocketGuildUser? guildUser = null)
        {
            var entity = new SocketThreadUser(guild, thread, model.UserId.Value, guildUser);
            entity.Update(model);
            return entity;
        }

        internal static SocketThreadUser Create(SocketGuild guild, SocketThreadChannel thread, SocketGuildUser? owner)
        {
            // this is used for creating the owner of the thread.
            var entity = new SocketThreadUser(guild, thread, owner?.Id ?? 0UL, owner);
            entity.Update(new Model
            {
                JoinTimestamp = thread.CreatedAt,
            });
            return entity;
        }

        internal void Update(Model model)
        {
            ThreadJoinedAt = model.JoinTimestamp;
            if (model.GuildMember.IsSpecified)
                GuildUser = Guild.AddOrUpdateUser(model.GuildMember.Value);
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <see cref="GuildUser"/> is <see langword="null"/>.
        /// </exception>
        public ChannelPermissions GetPermissions(IGuildChannel channel) => GuildUser?.GetPermissions(channel) ?? throw new InvalidOperationException("This thread user doesn't have an associated guild user!");

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <see cref="GuildUser"/> is <see langword="null"/>.
        /// </exception>
        public Task KickAsync(string? reason = null, RequestOptions? options = null) => GuildUser?.KickAsync(reason, options) ?? Task.FromException(new InvalidOperationException("This thread user doesn't have an associated guild user!"));

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <see cref="GuildUser"/> is <see langword="null"/>.
        /// </exception>
        public Task ModifyAsync(Action<GuildUserProperties> func, RequestOptions? options = null) => GuildUser?.ModifyAsync(func, options) ?? Task.FromException(new InvalidOperationException("This thread user doesn't have an associated guild user!"));

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <see cref="GuildUser"/> is <see langword="null"/>.
        /// </exception>
        public Task AddRoleAsync(ulong roleId, RequestOptions? options = null) => GuildUser?.AddRoleAsync(roleId, options) ?? Task.FromException(new InvalidOperationException("This thread user doesn't have an associated guild user!"));

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <see cref="GuildUser"/> is <see langword="null"/>.
        /// </exception>
        public Task AddRoleAsync(IRole role, RequestOptions? options = null) => GuildUser?.AddRoleAsync(role, options) ?? Task.FromException(new InvalidOperationException("This thread user doesn't have an associated guild user!"));

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <see cref="GuildUser"/> is <see langword="null"/>.
        /// </exception>
        public Task AddRolesAsync(IEnumerable<ulong> roleIds, RequestOptions? options = null) => GuildUser?.AddRolesAsync(roleIds, options) ?? Task.FromException(new InvalidOperationException("This thread user doesn't have an associated guild user!"));

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <see cref="GuildUser"/> is <see langword="null"/>.
        /// </exception>
        public Task AddRolesAsync(IEnumerable<IRole> roles, RequestOptions? options = null) => GuildUser?.AddRolesAsync(roles, options) ?? Task.FromException(new InvalidOperationException("This thread user doesn't have an associated guild user!"));

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <see cref="GuildUser"/> is <see langword="null"/>.
        /// </exception>
        public Task RemoveRoleAsync(ulong roleId, RequestOptions? options = null) => GuildUser?.RemoveRoleAsync(roleId, options) ?? Task.FromException(new InvalidOperationException("This thread user doesn't have an associated guild user!"));

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <see cref="GuildUser"/> is <see langword="null"/>.
        /// </exception>
        public Task RemoveRoleAsync(IRole role, RequestOptions? options = null) => GuildUser?.RemoveRoleAsync(role, options) ?? Task.FromException(new InvalidOperationException("This thread user doesn't have an associated guild user!"));

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <see cref="GuildUser"/> is <see langword="null"/>.
        /// </exception>
        public Task RemoveRolesAsync(IEnumerable<ulong> roleIds, RequestOptions? options = null) => GuildUser?.RemoveRolesAsync(roleIds, options) ?? Task.FromException(new InvalidOperationException("This thread user doesn't have an associated guild user!"));

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <see cref="GuildUser"/> is <see langword="null"/>.
        /// </exception>
        public Task RemoveRolesAsync(IEnumerable<IRole> roles, RequestOptions? options = null) => GuildUser?.RemoveRolesAsync(roles, options) ?? Task.FromException(new InvalidOperationException("This thread user doesn't have an associated guild user!"));
        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <see cref="GuildUser"/> is <see langword="null"/>.
        /// </exception>
        public Task SetTimeOutAsync(TimeSpan span, RequestOptions? options = null) => GuildUser?.SetTimeOutAsync(span, options) ?? Task.FromException(new InvalidOperationException("This thread user doesn't have an associated guild user!"));

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <see cref="GuildUser"/> is <see langword="null"/>.
        /// </exception>
        public Task RemoveTimeOutAsync(RequestOptions? options = null) => GuildUser?.RemoveTimeOutAsync(options) ?? Task.FromException(new InvalidOperationException("This thread user doesn't have an associated guild user!"));

        /// <inheritdoc/>
        IThreadChannel IThreadUser.Thread => Thread;

        /// <inheritdoc/>
        IGuild IThreadUser.Guild => Guild;

        /// <inheritdoc/>
        IGuild IGuildUser.Guild => Guild;

        /// <inheritdoc />
        IGuildUser? IThreadUser.GuildUser => GuildUser;

        /// <inheritdoc/>
        ulong IGuildUser.GuildId => Guild.Id;

        /// <inheritdoc/>
        GuildPermissions IGuildUser.GuildPermissions => GuildUser?.GuildPermissions ?? new GuildPermissions();

        /// <inheritdoc/>
        IReadOnlyCollection<ulong> IGuildUser.RoleIds => GuildUser?.Roles?.Select(x => x.Id)?.ToImmutableArray() ?? ImmutableArray<ulong>.Empty;

        /// <inheritdoc />
        string? IGuildUser.GetDisplayAvatarUrl(ImageFormat format, ushort size) => GuildUser?.GetDisplayAvatarUrl(format, size);

        /// <inheritdoc />
        string? IGuildUser.GetGuildAvatarUrl(ImageFormat format, ushort size) => GuildUser?.GetGuildAvatarUrl(format, size);

        /// <exception cref="InvalidOperationException">
        /// Thrown if <see cref="GuildUser"/> is <see langword="null"/>.
        /// </exception>
        internal override SocketGlobalUser GlobalUser
        {
            get => GuildUser?.GlobalUser ?? throw new InvalidOperationException("This thread user doesn't have an associated guild user!");
            set
            {
                if (GuildUser is null)
                {
                    throw new InvalidOperationException("This thread user doesn't have an associated guild user!");
                }
                GuildUser.GlobalUser = value;
            }
        }

        /// <exception cref="InvalidOperationException" accessor="set">
        /// Thrown if <see cref="GuildUser"/> is <see langword="null"/>.
        /// </exception>
        internal override SocketPresence? Presence
        {
            get => GuildUser?.Presence;
            set
            {
                if (GuildUser is null)
                {
                    throw new InvalidOperationException("This thread user doesn't have an associated guild user!");
                }
                GuildUser.Presence = value;
            }
        }

        /// <summary>
        ///     Gets the guild user of this thread user.
        /// </summary>
        /// <param name="user"></param>
        public static explicit operator SocketGuildUser?(SocketThreadUser user) => user.GuildUser;
    }
}
