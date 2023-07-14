using Discord;
using Discord.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Shims
{
    public class GuildUserShim : IGuildUserShim, ISnowflakeShimEntity
    {
        private readonly UserShim m_user;
        private readonly GuildShim m_guild;
        private readonly List<ulong> m_roleIds;
        private readonly IDiscordClientShim m_client;
        private string? m_nickname;

        public GuildUserShim(GuildShim guild) : this(guild, new UserShim(guild.Client))
        { }

        public GuildUserShim(GuildShim guild, UserShim user)
        {
            this.m_client = guild.Client;
            this.m_user = user;
            this.m_guild = guild;
            this.m_roleIds = new()
            {
                guild.Id
            };
        }

        public UserShim User => this.m_user;
        public GuildShim Guild => this.m_guild;

        protected List<ulong> RoleIds => this.m_roleIds;

        public string DisplayName => this.Nickname ?? this.GlobalName ?? this.Username;

        internal List<ulong> GetRoleIds() => this.RoleIds;

        public GuildPermissions GuildPermissions => new GuildPermissions(Permissions.ResolveGuild(this.Guild, this));

        public string? Nickname
        {
            get => this.m_nickname;
            set
            {
                value = value?.Trim()!;
                if (string.IsNullOrEmpty(value))
                {
                    this.m_nickname = null;
                    return;
                }

                value = value.Substring(0, Math.Min(value.Length, 32));
                this.m_nickname = value;
            }
        }
        public string? DisplayAvatarId => this.GuildAvatarId ?? this.AvatarId;
        public string? GuildAvatarId { get; set; }
        public bool IsBot
        {
            get => this.User.IsBot;
            set => this.User.IsBot = value;
        }
        public string Username
        {
            get => this.User.Username;
            set => this.User.Username = value;
        }
        public ushort DiscriminatorValue
        {
            get => this.User.DiscriminatorValue;
            set => this.User.DiscriminatorValue = value;
        }
        public string? AvatarId
        {
            get => this.User.AvatarId;
            set => this.User.AvatarId = value;
        }
        public bool IsWebhook => false;
        public bool IsSelfDeafened
        {
            get
            {
                return this.VoiceState?.IsSelfDeafened ?? false;
            }
            set
            {
                this.VoiceState ??= new();
                this.VoiceState.IsSelfDeafened = value;
            }
        }

        public bool IsSelfMuted
        {
            get
            {
                return this.VoiceState?.IsSelfMuted ?? false;
            }
            set
            {
                this.VoiceState ??= new();
                this.VoiceState.IsSelfMuted = value;
            }
        }

        public bool IsSuppressed
        {
            get
            {
                return this.VoiceState?.IsSuppressed ?? false;
            }
            set
            {
                this.VoiceState ??= new();
                this.VoiceState.IsSuppressed = value;
            }
        }

        public bool IsDeafened
        {
            get
            {
                return this.VoiceState?.IsDeafened ?? false;
            }
            set
            {
                this.VoiceState ??= new();
                this.VoiceState.IsDeafened = value;
            }
        }

        public bool IsMuted
        {
            get
            {
                return this.VoiceState?.IsMuted ?? false;
            }
            set
            {
                this.VoiceState ??= new();
                this.VoiceState.IsMuted = value;
            }
        }

        public bool IsStreaming
        {
            get
            {
                return this.VoiceState?.IsStreaming ?? false;
            }
            set
            {
                this.VoiceState ??= new();
                this.VoiceState.IsStreaming = value;
            }
        }

        public bool IsVideoing
        {
            get
            {
                return this.VoiceState?.IsVideoing ?? false;
            }
            set
            {
                this.VoiceState ??= new();
                this.VoiceState.IsVideoing = value;
            }
        }

        public DateTimeOffset? RequestToSpeakTimestamp
        {
            get
            {
                return this.VoiceState?.RequestToSpeakTimestamp ?? null;
            }
            set
            {
                this.VoiceState ??= new();
                this.VoiceState.RequestToSpeakTimestamp = value;
            }
        }

        public bool? IsPending { get; set; }

        public GuildUserFlags Flags { get; set; }

        public DateTimeOffset? JoinedAt { get; set; }
        public IReadOnlyCollection<IRole> Roles
            => this.m_roleIds.Select(id => this.Guild.GetRole(id)!).Where(x => x is not null).ToArray();
        public IVoiceChannel? VoiceChannel
        {
            get => this.VoiceState?.VoiceChannel;
            set
            {
                this.VoiceState ??= new();
                this.VoiceState.VoiceChannel = value;
            }
        }
        public string VoiceSessionId
        {
            get => this.VoiceState?.VoiceSessionId ?? "";
            set
            {
                this.VoiceState ??= new();
                this.VoiceState.VoiceSessionId = value;
            }
        }

        public VoiceStateShim? VoiceState { get; set; }
        public AudioInStream? AudioStream => null;
        public DateTimeOffset? PremiumSince { get; set; }
        public DateTimeOffset? TimedOutUntil { get; set; }

        public int Hierarchy
        {
            get
            {
                if (this.Guild.OwnerId == this.Id)
                    return int.MaxValue;

                int maxPos = 0;
                for (int i = 0; i < this.m_roleIds.Count; i++)
                {
                    RoleShim? role = this.Guild.GetRole(this.m_roleIds[i]);
                    if (role != null && role.Position > maxPos)
                        maxPos = role.Position;
                }
                return maxPos;
            }
        }


        public virtual async Task ModifyAsync(Action<GuildUserProperties> func, RequestOptions? options = null)
        {
            GuildUserProperties properties = new();
            func(properties);

            if (properties.Nickname.IsSpecified)
            {
                this.Nickname = properties.Nickname.Value;
            }
            if (properties.Channel.IsSpecified)
            {
                this.VoiceState ??= new VoiceStateShim();
                this.VoiceState.VoiceChannel = properties.Channel.Value;
            }
            if (properties.Deaf.IsSpecified)
            {
                this.VoiceState ??= new VoiceStateShim();
                this.VoiceState.IsDeafened = properties.Deaf.Value;
            }
            if (properties.Mute.IsSpecified)
            {
                this.VoiceState ??= new VoiceStateShim();
                this.VoiceState.IsMuted = properties.Mute.Value;
            }
            if (properties.ChannelId.IsSpecified)
            {
                this.VoiceState ??= new VoiceStateShim();
                if (!properties.ChannelId.Value.HasValue)
                {
                    this.VoiceState.VoiceChannel = null;
                }
                else
                {
                    this.VoiceState.VoiceChannel = await this.Guild.GetVoiceChannelAsync(properties.ChannelId.Value.Value);
                }
            }
            if (properties.Flags.IsSpecified)
            {
                this.Flags = properties.Flags.Value;
            }
            if (properties.RoleIds.IsSpecified)
            {
                this.m_roleIds.Clear();
                this.m_roleIds.AddRange(properties.RoleIds.Value.Where(this.Guild.HasRole));
            }
            if (properties.Roles.IsSpecified)
            {
                this.m_roleIds.Clear();
                this.m_roleIds.AddRange(properties.Roles.Value
                    .Where(r => r is not null)
                    .Select(r => r.Id));
            }
            if (properties.TimedOutUntil.IsSpecified)
            {
                this.TimedOutUntil = properties.TimedOutUntil.Value;
            }
        }
        /// <inheritdoc />
        public virtual Task KickAsync(string? reason = null, RequestOptions? options = null)
        {

        }
        /// <inheritdoc />
        public virtual Task AddRoleAsync(ulong roleId, RequestOptions? options = null)
        {
            return this.AddRolesAsync(new[] { roleId }, options);
        }
        /// <inheritdoc />
        public virtual Task AddRoleAsync(IRole role, RequestOptions? options = null)
        {
            return this.AddRoleAsync(role.Id, options);
        }
        /// <inheritdoc />
        public virtual Task AddRolesAsync(IEnumerable<ulong> roleIds, RequestOptions? options = null)
        {
            foreach (ulong roleId in roleIds)
            {
                if (!this.Guild.HasRole(roleId) || this.RoleIds.Contains(roleId))
                {
                    continue;
                }
                this.RoleIds.Add(roleId);
            }
            return Task.CompletedTask;
        }
        /// <inheritdoc />
        public virtual Task AddRolesAsync(IEnumerable<IRole> roles, RequestOptions? options = null)
        {
            return this.AddRolesAsync(roles.Select(x => x.Id), options);
        }
        /// <inheritdoc />
        public virtual Task RemoveRoleAsync(ulong roleId, RequestOptions? options = null)
        {
            return this.RemoveRolesAsync(new[] { roleId }, options);
        }
        /// <inheritdoc />
        public virtual Task RemoveRoleAsync(IRole role, RequestOptions? options = null)
        {
            return this.RemoveRoleAsync(role.Id, options);
        }
        /// <inheritdoc />
        public virtual Task RemoveRolesAsync(IEnumerable<ulong> roleIds, RequestOptions? options = null)
        {
            foreach (ulong roleId in roleIds)
            {
                this.RoleIds.Remove(roleId);
            }
            return Task.CompletedTask;
        }
        /// <inheritdoc />
        public virtual Task RemoveRolesAsync(IEnumerable<IRole> roles, RequestOptions? options = null)
        {
            return this.RemoveRolesAsync(roles.Select(x => x.Id));
        }
        /// <inheritdoc />
        public virtual Task SetTimeOutAsync(TimeSpan span, RequestOptions? options = null)
        {
            this.TimedOutUntil = DateTimeOffset.Now + span;
            return Task.CompletedTask;
        }
        /// <inheritdoc />
        public Task RemoveTimeOutAsync(RequestOptions? options = null)
        {
            this.TimedOutUntil = null;
            return Task.CompletedTask;
        }
        /// <inheritdoc />
        public ChannelPermissions GetPermissions(IGuildChannel channel)
        {
            return new ChannelPermissions(Permissions.ResolveChannel(this.Guild, this, channel, this.GuildPermissions.RawValue));
        }

        /// <inheritdoc />
        public string? GetDisplayAvatarUrl(ImageFormat format = ImageFormat.Auto, ushort size = 128)
        {
            if (this.GuildAvatarId is not null)
            {
                return this.GetGuildAvatarUrl(format, size);
            }
            else
            {
                return this.GetAvatarUrl(format, size);
            }
        }

        /// <inheritdoc />
        public string? GetGuildAvatarUrl(ImageFormat format = ImageFormat.Auto, ushort size = 128)
        {
            return CDN.GetGuildUserAvatarUrl(this.Id, this.Guild.Id, this.GuildAvatarId, size, format);
        }

        #region User

        public string? GlobalName
        {
            get => this.User.GlobalName;
            set => this.User.GlobalName = value;
        }
        public ulong Id
        {
            get => this.User.Id;
            set => this.User.Id = value;
        }
        public string Discriminator
        {
            get => this.User.Discriminator;
            set => this.User.Discriminator = value;
        }
        public string Mention
        {
            get => this.User.Mention;
        }
        public UserProperties? PublicFlags
        {
            get => this.User.PublicFlags;
            set => this.User.PublicFlags = value;
        }
        public string? Pronouns
        {
            get => this.User.Pronouns;
            set => this.User.Pronouns = value;
        }
        public DateTimeOffset CreatedAt
        {
            get => this.User.CreatedAt;
        }
        public UserStatus Status
        {
            get => this.User.Status;
            set => this.User.Status = value;
        }
        public List<ClientType> ActiveClients
        {
            get => this.User.ActiveClients;
        }
        public List<IActivityShim> Activities
        {
            get => this.User.Activities;
        }

        public Task<IDMChannel> CreateDMChannelAsync(RequestOptions? options = null)
        {
            return this.User.CreateDMChannelAsync(options);
        }

        public string? GetAvatarUrl(ImageFormat format = ImageFormat.Auto, ushort size = 128)
        {
            return this.User.GetAvatarUrl(format, size);
        }

        public string GetDefaultAvatarUrl()
        {
            return this.User.GetDefaultAvatarUrl();
        }

        IReadOnlyCollection<ClientType> IPresence.ActiveClients => ((IPresence)this.User).ActiveClients;
        IReadOnlyCollection<IActivity> IPresence.Activities => ((IPresence)this.User).Activities;


        #endregion

        #region IGuildUser

        /// <inheritdoc />
        IGuild IGuildUser.Guild => this.Guild;
        ulong IGuildUser.GuildId => this.Guild.Id;
        IReadOnlyCollection<ulong> IGuildUser.RoleIds => this.m_roleIds.AsReadOnly();
        IVoiceChannel? IVoiceState.VoiceChannel => this.VoiceChannel;
        #endregion
    }
}
