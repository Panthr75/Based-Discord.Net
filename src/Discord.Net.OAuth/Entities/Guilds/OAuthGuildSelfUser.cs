using Discord.API;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Discord.OAuth;

internal sealed class OAuthGuildSelfUser : OAuthGuildUser
{
    private bool isOwner;

    public OAuthGuildSelfUser(OAuthGuild guild, OAuthGlobalUser globalUser)
        : base(guild, globalUser)
    { }

    public sealed override bool IsOwner => this.isOwner;
    public sealed override Optional<string?> Nickname
        => this.FetchedGuildUser is null ?
            Optional<string?>.Unspecified :
            this.FetchedGuildUser.Nickname;

    public sealed override Optional<string?> GuildAvatarId
        => this.FetchedGuildUser is null ?
            Optional<string?>.Unspecified :
            this.FetchedGuildUser.Nickname;

    public sealed override Optional<bool?> IsPending
        => this.FetchedGuildUser is null ?
            Optional<bool?>.Unspecified :
            this.FetchedGuildUser.IsPending;

    public sealed override Optional<DateTimeOffset?> JoinedAt
        => this.FetchedGuildUser is null ?
            Optional<DateTimeOffset?>.Unspecified :
            this.FetchedGuildUser.JoinedAt;

    public sealed override Optional<GuildUserFlags> MemberFlags
        => this.FetchedGuildUser is null ?
            Optional<GuildUserFlags>.Unspecified :
            this.FetchedGuildUser.MemberFlags;

    public sealed override Optional<IReadOnlyCollection<ulong>> RoleIds
        => this.FetchedGuildUser is null ?
            Optional<IReadOnlyCollection<ulong>>.Unspecified :
            Optional.Create(this.FetchedGuildUser.RoleIds);

    public sealed override Optional<IReadOnlyCollection<IRole>> Roles
        => this.FetchedGuildUser is null ?
            Optional<IReadOnlyCollection<IRole>>.Unspecified :
            Optional.Create(this.FetchedGuildUser.Roles);

    public sealed override Optional<DateTimeOffset?> PremiumSince
        => this.FetchedGuildUser is null ?
            Optional<DateTimeOffset?>.Unspecified :
            this.FetchedGuildUser.PremiumSince;

    public sealed override Optional<DateTimeOffset?> TimedOutUntil
        => this.FetchedGuildUser is null ?
            Optional<DateTimeOffset?>.Unspecified :
            this.FetchedGuildUser.TimedOutUntil;

    public sealed override Optional<bool> IsMuted
        => this.FetchedGuildUser is null ?
            Optional<bool>.Unspecified :
            this.FetchedGuildUser.IsMuted;

    public sealed override Optional<bool> IsDeafened
        => this.FetchedGuildUser is null ?
            Optional<bool>.Unspecified :
            this.FetchedGuildUser.IsDeafened;

    public sealed override Optional<int> Hierarchy
        => this.FetchedGuildUser is null ?
            Optional<int>.Unspecified :
            this.FetchedGuildUser.Hierarchy;

    public sealed override bool Fetched => this.FetchedGuildUser is not null;

    private FetchedGuildUserInfo? FetchedGuildUser { get; set; }

    internal void Update(UserGuild userGuild)
    {
        this.isOwner = userGuild.Owner;
        this.GuildPermissions = new GuildPermissions(userGuild.Permissions);
    }

    internal void Update(GuildMember model)
    {
        this.FetchedGuildUser ??= new();
        this.FetchedGuildUser.Update(this.Guild, this.IsOwner, model);
    }

    internal void Update(SocketGuildUser guildUser)
    {
        this.FetchedGuildUser ??= new();
        this.FetchedGuildUser.Update(this.Guild, guildUser);
    }

    internal void Update(RestGuildUser guildUser)
    {
        this.FetchedGuildUser ??= new();
        this.FetchedGuildUser.Update(this.Guild, guildUser);
    }

    internal void Update(IGuildUser guildUser)
    {
        this.FetchedGuildUser ??= new();
        this.FetchedGuildUser.Update(this.Guild, guildUser);
    }

    private async Task UpdateGuildUserFromUnknownClientAsync(IDiscordClient client, RequestOptions? options = null)
    {
        IGuild? guild = await client.GetGuildAsync(this.GuildId, CacheMode.AllowDownload, options).ConfigureAwait(false);
        if (guild is null)
            throw new NotSupportedException();

        this.Guild.Update(guild);

        IGuildUser? user = await guild.GetUserAsync(this.Id, CacheMode.AllowDownload, options).ConfigureAwait(false);
        if (user is null)
            throw new NotSupportedException();

        this.Update(user);
    }

    public sealed override async Task UpdateGuildUserFromClientAsync(DiscordRestClient client, RequestOptions? options = null)
    {
        RestGuild? guild = await client.GetGuildAsync(this.GuildId, options).ConfigureAwait(false);
        if (guild is null)
            throw new NotSupportedException();

        this.Guild.Update(guild);

        RestGuildUser? user = await guild.GetUserAsync(this.Id, options).ConfigureAwait(false);
        if (user is null)
            throw new NotSupportedException();

        this.Update(user);
    }

    public sealed override Task UpdateGuildUserFromClientAsync(BaseSocketClient client, RequestOptions? options = null)
    {
        SocketGuild? guild = client.GetGuild(this.GuildId);
        if (guild is null)
            return Task.FromException(new NotSupportedException());

        this.Guild.Update(guild);

        SocketGuildUser? user = guild.GetUser(this.Id);
        if (user is null)
            return Task.FromException(new NotSupportedException());

        this.Update(user);
        return Task.FromResult(true);
    }

    public sealed override Task UpdateGuildUserFromClientAsync(IDiscordClient client, RequestOptions? options = null)
    {
        if (client is DiscordRestClient restClient)
            return this.UpdateGuildUserFromClientAsync(restClient, options);
        else if (client is BaseSocketClient socketClient)
            return this.UpdateGuildUserFromClientAsync(socketClient, options);
        else
            return this.UpdateGuildUserFromUnknownClientAsync(client, options);
    }

    public sealed override Task UpdateGuildUserFromOAuthAsync(RequestOptions? options = null)
    {
        return this.AccessClient.UpdateCurrentMemberInGuildAsync(this.Guild, options);
    }

    private sealed class FetchedGuildUserInfo
    {
        private ImmutableArray<ulong> roleIds;
        private ImmutableArray<IRole> roles;

        public string? Nickname { get; set; }
        public string? GuildAvatarId { get; set; }
        public bool? IsPending { get; set; }
        public DateTimeOffset? JoinedAt { get; set; }
        public IReadOnlyCollection<ulong> RoleIds => this.roleIds;
        public IReadOnlyCollection<IRole> Roles => this.roles;
        public DateTimeOffset? PremiumSince { get; set; }
        public DateTimeOffset? TimedOutUntil { get; set; }
        public bool IsMuted { get; set; }
        public bool IsDeafened { get; set; }
        public GuildUserFlags MemberFlags { get; set; }
        public int Hierarchy { get; set; }

        public void Update(OAuthGuild guild, bool isOwner, GuildMember model)
        {
            if (model.Nick.IsSpecified)
                this.Nickname = model.Nick.Value;
            if (model.Avatar.IsSpecified)
                this.GuildAvatarId = model.Avatar.Value;
            if (model.Pending.IsSpecified)
                this.IsPending = model.Pending.Value;
            if (model.JoinedAt.IsSpecified)
                this.JoinedAt = model.JoinedAt.Value;
            if (model.Roles.IsSpecified)
            {
                this.roleIds = model.Roles.Value.ToImmutableArray();
                if (this.roleIds.Length > 0)
                {
                    ImmutableArray<IRole>.Builder builder = ImmutableArray.CreateBuilder<IRole>(this.roleIds.Length);
                    foreach (ulong roleId in this.roleIds)
                    {
                        Optional<IRole?> role = guild.GetRole(roleId);
                        if (role.IsSpecified && role.Value is not null)
                        {
                            builder.Add(role.Value);
                        }
                    }
                    this.roles = builder.ToImmutable();
                }
                else
                {
                    this.roles = ImmutableArray<IRole>.Empty;
                }
            }
            if (model.PremiumSince.IsSpecified)
                this.PremiumSince = model.PremiumSince.Value;
            if (model.TimedOutUntil.IsSpecified)
                this.TimedOutUntil = model.TimedOutUntil.Value;
            if (model.Mute.IsSpecified)
                this.IsMuted = model.Mute.Value;
            if (model.Deaf.IsSpecified)
                this.IsDeafened = model.Deaf.Value;
            this.MemberFlags = model.Flags;

            this.Hierarchy = isOwner ?
                int.MaxValue :
                this.roles.Select(x => x.Position).DefaultIfEmpty().Max();

        }

        public void Update(OAuthGuild guild, SocketGuildUser guildUser)
        {
            this.Nickname = guildUser.Nickname;
            this.GuildAvatarId = guildUser.GuildAvatarId;
            this.IsPending = guildUser.IsPending;
            this.JoinedAt = guildUser.JoinedAt;
            this.roleIds = guildUser.Roles.Select(x => x.Id).ToImmutableArray();
            if (this.roleIds.Length > 0)
            {
                ImmutableArray<IRole>.Builder builder = ImmutableArray.CreateBuilder<IRole>(this.roleIds.Length);
                foreach (ulong roleId in this.roleIds)
                {
                    Optional<IRole?> role = guild.GetRole(roleId);
                    if (role.IsSpecified && role.Value is not null)
                    {
                        builder.Add(role.Value);
                    }
                }
                this.roles = builder.ToImmutable();
            }
            else
            {
                this.roles = ImmutableArray<IRole>.Empty;
            }
            this.PremiumSince = guildUser.PremiumSince;
            this.TimedOutUntil = guildUser.TimedOutUntil;
            this.IsMuted = guildUser.IsMuted;
            this.IsDeafened = guildUser.IsDeafened;
            this.MemberFlags = guildUser.Flags;
            this.Hierarchy = guildUser.Hierarchy;
        }

        public void Update(OAuthGuild guild, RestGuildUser guildUser)
        {
            this.Nickname = guildUser.Nickname;
            this.GuildAvatarId = guildUser.GuildAvatarId;
            this.IsPending = guildUser.IsPending;
            this.JoinedAt = guildUser.JoinedAt;
            this.roleIds = guildUser.RoleIds.ToImmutableArray();
            if (this.roleIds.Length > 0)
            {
                ImmutableArray<IRole>.Builder builder = ImmutableArray.CreateBuilder<IRole>(this.roleIds.Length);
                foreach (ulong roleId in this.roleIds)
                {
                    Optional<IRole?> role = guild.GetRole(roleId);
                    if (role.IsSpecified && role.Value is not null)
                    {
                        builder.Add(role.Value);
                    }
                }
                this.roles = builder.ToImmutable();
            }
            else
            {
                this.roles = ImmutableArray<IRole>.Empty;
            }
            this.PremiumSince = guildUser.PremiumSince;
            this.TimedOutUntil = guildUser.TimedOutUntil;
            this.IsMuted = guildUser.IsMuted;
            this.IsDeafened = guildUser.IsDeafened;
            this.MemberFlags = guildUser.Flags;
            this.Hierarchy = guildUser.Hierarchy;
        }

        public void Update(OAuthGuild guild, IGuildUser guildUser)
        {
            this.Nickname = guildUser.Nickname;
            this.GuildAvatarId = guildUser.GuildAvatarId;
            this.IsPending = guildUser.IsPending;
            this.JoinedAt = guildUser.JoinedAt;
            this.roleIds = guildUser.RoleIds.ToImmutableArray();
            if (this.roleIds.Length > 0)
            {
                ImmutableArray<IRole>.Builder builder = ImmutableArray.CreateBuilder<IRole>(this.roleIds.Length);
                foreach (ulong roleId in this.roleIds)
                {
                    Optional<IRole?> role = guild.GetRole(roleId);
                    if (role.IsSpecified && role.Value is not null)
                    {
                        builder.Add(role.Value);
                    }
                }
                this.roles = builder.ToImmutable();
            }
            else
            {
                this.roles = ImmutableArray<IRole>.Empty;
            }
            this.PremiumSince = guildUser.PremiumSince;
            this.TimedOutUntil = guildUser.TimedOutUntil;
            this.IsMuted = guildUser.IsMuted;
            this.IsDeafened = guildUser.IsDeafened;
            this.MemberFlags = guildUser.Flags;
            this.Hierarchy = guildUser.Hierarchy;
        }
    }
}
