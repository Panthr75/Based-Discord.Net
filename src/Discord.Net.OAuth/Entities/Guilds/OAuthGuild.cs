using Discord.API;
using Discord.Audio;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Discord.OAuth;

/// <summary>
/// A representation of an OAuth Guild.
/// </summary>
public sealed class OAuthGuild : IGuild, IUserGuild
{
    private readonly EveryoneRoleWrapper everyoneRoleWrapper;
    private FetchedGuildInfo? fetchedGuild;
    private TempCurrentUserInformation tempCurrentUserInformation;

    internal OAuthGuild(UserGuild model, OAuthAccessClient accessClient)
    {
        this.AccessClient = accessClient;
        this.tempCurrentUserInformation = new(model);
        this.everyoneRoleWrapper = new(this);
        this.Id = model.Id;
        this.Name = model.Name;
        this.IconId = model.Icon;
        this.Features = model.Features;
        this.ApproximateMemberCount = model.ApproximateMemberCount.ToNullable();
        this.ApproximatePresenceCount = model.ApproximatePresenceCount.ToNullable();
        this.fetchedGuild = null;
    }

    /// <summary>
    /// The access client that created this guild.
    /// </summary>
    public OAuthAccessClient AccessClient { get; }

    /// <summary>
    /// The current guild user.
    /// </summary>
    public OAuthGuildUser? CurrentUser => this.CurrentUserSelf;

    /// <summary>
    /// Gets whether or not the current user is the guild
    /// owner.
    /// </summary>
    public bool CurrentUserIsOwner => this.CurrentUserSelf?.IsOwner ?? this.tempCurrentUserInformation!.IsOwner;

    /// <summary>
    /// Gets the permissions of the current user.
    /// </summary>
    public GuildPermissions CurrentUserPermissions => this.CurrentUserSelf?.GuildPermissions ?? this.tempCurrentUserInformation!.GetPermissions();

    internal OAuthGuildSelfUser? CurrentUserSelf { get; private set; }

    /// <inheritdoc/>
    public ulong Id { get; }
    /// <inheritdoc/>
    public DateTimeOffset CreatedAt => SnowflakeUtils.FromSnowflake(this.Id);
    /// <inheritdoc/>
    public string Name { get; internal set; }
    /// <inheritdoc/>
    public string? IconId { get; internal set; }
    /// <inheritdoc/>
    public string? IconUrl => CDN.GetGuildIconUrl(this.Id, this.IconUrl);
    /// <inheritdoc/>
    public GuildFeatures Features { get; internal set; }
    /// <inheritdoc/>
    public int? ApproximateMemberCount { get; internal set; }
    /// <inheritdoc/>
    public int? ApproximatePresenceCount { get; internal set; }

    /// <summary>
    /// Gets whether or not this guild has been fetched by a
    /// discord client.
    /// </summary>
    public bool Fetched => this.fetchedGuild is not null;

    /// <inheritdoc cref="IGuild.AFKTimeout"/>
    public Optional<int> AFKTimeout
        => this.fetchedGuild is null ? 
            Optional<int>.Unspecified :
            this.fetchedGuild.AfkTimeout;
    /// <inheritdoc cref="IGuild.IsWidgetEnabled"/>
    public Optional<bool> IsWidgetEnabled
        => this.fetchedGuild is null ? 
            Optional<bool>.Unspecified :
            this.fetchedGuild.IsWidgetEnabled;
    /// <inheritdoc cref="IGuild.DefaultMessageNotifications"/>
    public Optional<DefaultMessageNotifications> DefaultMessageNotifications
        => this.fetchedGuild is null ?
            Optional<DefaultMessageNotifications>.Unspecified :
            this.fetchedGuild.DefaultMessageNotifications;
    /// <inheritdoc cref="IGuild.MfaLevel"/>
    public Optional<MfaLevel> MfaLevel
        => this.fetchedGuild is null ?
            Optional<MfaLevel>.Unspecified :
            this.fetchedGuild.MfaLevel;
    /// <inheritdoc cref="IGuild.VerificationLevel"/>
    public Optional<VerificationLevel> VerificationLevel
        => this.fetchedGuild is null ? 
            Optional<VerificationLevel>.Unspecified :
            this.fetchedGuild.VerificationLevel;
    /// <inheritdoc cref="IGuild.ExplicitContentFilter"/>
    public Optional<ExplicitContentFilterLevel> ExplicitContentFilter
        => this.fetchedGuild is null ?
            Optional<ExplicitContentFilterLevel>.Unspecified :
            this.fetchedGuild.ExplicitContentFilter;
    /// <inheritdoc cref="IGuild.SplashId"/>
    public Optional<string?> SplashId
        => this.fetchedGuild is null ?
            Optional<string?>.Unspecified :
            this.fetchedGuild.SplashId;
    /// <inheritdoc cref="IGuild.SplashUrl"/>
    public Optional<string?> SplashUrl
        => this.fetchedGuild is null ?
            Optional<string?>.Unspecified :
            this.fetchedGuild.SplashUrl;
    /// <inheritdoc cref="IGuild.DiscoverySplashId"/>
    public Optional<string?> DiscoverySplashId
        => this.fetchedGuild is null ?
            Optional<string?>.Unspecified :
            this.fetchedGuild.DiscoverySplashId;
    /// <inheritdoc cref="IGuild.DiscoverySplashUrl"/>
    public Optional<string?> DiscoverySplashUrl
        => this.fetchedGuild is null ?
            Optional<string?>.Unspecified :
            this.fetchedGuild.DiscoverySplashUrl;
    /// <inheritdoc cref="IGuild.AFKChannelId"/>
    public Optional<ulong?> AFKChannelId
        => this.fetchedGuild is null ?
            Optional<ulong?>.Unspecified :
            this.fetchedGuild.AFKChannelId;
    /// <inheritdoc cref="IGuild.WidgetChannelId"/>
    public Optional<ulong?> WidgetChannelId
        => this.fetchedGuild is null ?
            Optional<ulong?>.Unspecified :
            this.fetchedGuild.WidgetChannelId;
    /// <inheritdoc cref="IGuild.SafetyAlertsChannelId"/>
    public Optional<ulong?> SafetyAlertsChannelId
        => this.fetchedGuild is null ?
            Optional<ulong?>.Unspecified :
            this.fetchedGuild.SafetyAlertsChannelId;
    /// <inheritdoc cref="IGuild.SystemChannelId"/>
    public Optional<ulong?> SystemChannelId
        => this.fetchedGuild is null ?
            Optional<ulong?>.Unspecified :
            this.fetchedGuild.SystemChannelId;
    /// <inheritdoc cref="IGuild.RulesChannelId"/>
    public Optional<ulong?> RulesChannelId
        => this.fetchedGuild is null ?
            Optional<ulong?>.Unspecified :
            this.fetchedGuild.RulesChannelId;
    /// <inheritdoc cref="IGuild.PublicUpdatesChannelId"/>
    public Optional<ulong?> PublicUpdatesChannelId
        => this.fetchedGuild is null ?
            Optional<ulong?>.Unspecified :
            this.fetchedGuild.PublicUpdatesChannelId;
    /// <inheritdoc cref="IGuild.OwnerId"/>
    public Optional<ulong> OwnerId
        => this.fetchedGuild is null ?
            Optional<ulong>.Unspecified :
            this.fetchedGuild.OwnerId;
    /// <inheritdoc cref="IGuild.ApplicationId"/>
    public Optional<ulong?> ApplicationId
        => this.fetchedGuild is null ?
            Optional<ulong?>.Unspecified :
            this.fetchedGuild.ApplicationId;
    /// <inheritdoc cref="IGuild.EveryoneRole"/>
    public Optional<IRole> EveryoneRole
        => this.fetchedGuild is null ?
            Optional<IRole>.Unspecified :
            Optional.Create(this.fetchedGuild.EveryoneRole);
    /// <inheritdoc cref="IGuild.Emotes"/>
    public Optional<IReadOnlyCollection<GuildEmote>> Emotes
        => this.fetchedGuild is null ?
            Optional<IReadOnlyCollection<GuildEmote>>.Unspecified :
            Optional.Create(this.fetchedGuild.Emotes);
    /// <inheritdoc cref="IGuild.Stickers"/>
    public Optional<IReadOnlyCollection<ICustomSticker>> Stickers
        => this.fetchedGuild is null ?
            Optional<IReadOnlyCollection<ICustomSticker>>.Unspecified :
            Optional.Create(this.fetchedGuild.Stickers);
    /// <inheritdoc cref="IGuild.Roles"/>
    public Optional<IReadOnlyCollection<IRole>> Roles
        => this.fetchedGuild is null ?
            Optional<IReadOnlyCollection<IRole>>.Unspecified :
            Optional.Create(this.fetchedGuild.Roles);
    /// <inheritdoc cref="IGuild.PremiumTier"/>
    public Optional<PremiumTier> PremiumTier
        => this.fetchedGuild is null ?
            Optional<PremiumTier>.Unspecified :
            this.fetchedGuild.PremiumTier;
    /// <inheritdoc cref="IGuild.BannerId"/>
    public Optional<string?> BannerId
        => this.fetchedGuild is null ?
            Optional<string?>.Unspecified :
            this.fetchedGuild.BannerId;
    /// <inheritdoc cref="IGuild.BannerUrl"/>
    public Optional<string?> BannerUrl
        => this.fetchedGuild is null ?
            Optional<string?>.Unspecified :
            this.fetchedGuild.BannerUrl;
    /// <inheritdoc cref="IGuild.VanityURLCode"/>
    public Optional<string?> VanityURLCode
        => this.fetchedGuild is null ?
            Optional<string?>.Unspecified :
            this.fetchedGuild.VanityURLCode;
    /// <inheritdoc cref="IGuild.SystemChannelFlags"/>
    public Optional<SystemChannelMessageDeny> SystemChannelFlags
        => this.fetchedGuild is null ?
            Optional<SystemChannelMessageDeny>.Unspecified :
            this.fetchedGuild.SystemChannelFlags;
    /// <inheritdoc cref="IGuild.Description"/>
    public Optional<string?> Description
        => this.fetchedGuild is null ?
            Optional<string?>.Unspecified :
            this.fetchedGuild.Description;
    /// <inheritdoc cref="IGuild.PremiumSubscriptionCount"/>
    public Optional<int> PremiumSubscriptionCount
        => this.fetchedGuild is null ?
            Optional<int>.Unspecified :
            this.fetchedGuild.PremiumSubscriptionCount;
    /// <inheritdoc cref="IGuild.MaxPresences"/>
    public Optional<int?> MaxPresences
        => this.fetchedGuild is null ?
            Optional<int?>.Unspecified :
            this.fetchedGuild.MaxPresences;
    /// <inheritdoc cref="IGuild.MaxMembers"/>
    public Optional<int?> MaxMembers
        => this.fetchedGuild is null ?
            Optional<int?>.Unspecified :
            this.fetchedGuild.MaxMembers;
    /// <inheritdoc cref="IGuild.MaxVideoChannelUsers"/>
    public Optional<int?> MaxVideoChannelUsers
        => this.fetchedGuild is null ?
            Optional<int?>.Unspecified :
            this.fetchedGuild.MaxVideoChannelUsers;
    /// <inheritdoc cref="IGuild.MaxStageVideoChannelUsers"/>
    public Optional<int?> MaxStageVideoChannelUsers
        => this.fetchedGuild is null ?
            Optional<int?>.Unspecified :
            this.fetchedGuild.MaxStageVideoChannelUsers;
    /// <inheritdoc cref="IGuild.MaxBitrate"/>
    public Optional<int> MaxBitrate
        => this.fetchedGuild is null ?
            Optional<int>.Unspecified :
            this.fetchedGuild.MaxBitrate;
    /// <inheritdoc cref="IGuild.PreferredLocale"/>
    public Optional<string> PreferredLocale
        => this.fetchedGuild is null ? 
            Optional<string>.Unspecified :
            this.fetchedGuild.PreferredLocale;
    /// <inheritdoc cref="IGuild.NsfwLevel"/>
    public Optional<NsfwLevel> NsfwLevel
        => this.fetchedGuild is null ? 
            Optional<NsfwLevel>.Unspecified :
            this.fetchedGuild.NsfwLevel;
    /// <inheritdoc cref="IGuild.PreferredCulture"/>
    public Optional<CultureInfo> PreferredCulture
        => this.fetchedGuild is null ? 
            Optional<CultureInfo>.Unspecified :
            this.fetchedGuild.PreferredCulture;
    /// <inheritdoc cref="IGuild.IsBoostProgressBarEnabled"/>
    public Optional<bool> IsBoostProgressBarEnabled
        => this.fetchedGuild is null ? 
            Optional<bool>.Unspecified :
            this.fetchedGuild.IsBoostProgressBarEnabled;
    /// <inheritdoc cref="IGuild.MaxUploadLimit"/>
    public Optional<ulong> MaxUploadLimit
        => this.fetchedGuild is null ? 
            Optional<ulong>.Unspecified :
            this.fetchedGuild.MaxUploadLimit;
    /// <inheritdoc cref="IGuild.InventorySettings"/>
    public Optional<GuildInventorySettings?> InventorySettings
        => this.fetchedGuild is null ? 
            Optional<GuildInventorySettings?>.Unspecified :
            this.fetchedGuild.InventorySettings;
    /// <inheritdoc cref="IGuild.IncidentsData"/>
    public Optional<GuildIncidentsData> IncidentsData
        => this.fetchedGuild is null ? 
            Optional<GuildIncidentsData>.Unspecified :
            this.fetchedGuild.IncidentsData;

    /// <summary>
    /// Gets the role with the specified id, if this guild has
    /// been fetched.
    /// </summary>
    /// <param name="id">
    /// The ID of the role to get.
    /// </param>
    /// <returns>
    /// An optional <see cref="IRole"/>, where it is
    /// unspecified if the guild hasn't been fetched, otherwise
    /// will contain the role fetched, or
    /// <see langword="null"/> if no role with id
    /// <paramref name="id"/> was found.
    /// </returns>
    public Optional<IRole?> GetRole(ulong id)
    {
        if (this.fetchedGuild is null)
        {
            if (this.Id == id)
                return this.everyoneRoleWrapper;
            else
                return Optional<IRole?>.Unspecified;
        }

        return Optional.Create(this.fetchedGuild.GetRole(id));
    }

    internal void Update(UserGuild model)
    {
        this.Name = model.Name;
        this.IconId = model.Icon;
        this.Features = model.Features;
        if (model.ApproximateMemberCount.IsSpecified)
            this.ApproximateMemberCount = model.ApproximateMemberCount.Value;
        if (model.ApproximatePresenceCount.IsSpecified)
            this.ApproximatePresenceCount = model.ApproximatePresenceCount.Value;

        if (this.CurrentUserSelf is null)
        {
            this.tempCurrentUserInformation = new(model);
        }
        else
        {
            this.CurrentUserSelf.Update(model);
        }
    }

    internal void Update(Guild model)
    {
        this.Name = model.Name;
        this.IconId = model.Icon;
        this.Features = model.Features;
        if (model.ApproximateMemberCount.IsSpecified)
            this.ApproximateMemberCount = model.ApproximateMemberCount.Value;
        if (model.ApproximatePresenceCount.IsSpecified)
            this.ApproximatePresenceCount = model.ApproximatePresenceCount.Value;

        this.fetchedGuild ??= new FetchedGuildInfo(this);
        this.fetchedGuild.Update(model);
    }

    internal void Update(SocketGuild guild)
    {
        this.Name = guild.Name;
        this.IconId = guild.IconId;
        this.Features = guild.Features;

        this.fetchedGuild ??= new FetchedGuildInfo(this);
        this.fetchedGuild.Update(guild);
    }

    internal void Update(RestGuild guild)
    {
        this.Name = guild.Name;
        this.IconId = guild.IconId;
        this.Features = guild.Features;
        if (guild.ApproximateMemberCount.HasValue)
            this.ApproximateMemberCount = guild.ApproximateMemberCount.Value;
        if (guild.ApproximatePresenceCount.HasValue)
            this.ApproximatePresenceCount = guild.ApproximatePresenceCount.Value;

        this.fetchedGuild ??= new FetchedGuildInfo(this);
        this.fetchedGuild.Update(guild);
    }

    internal void Update(IGuild guild)
    {
        this.Name = guild.Name;
        this.IconId = guild.IconId;
        this.Features = guild.Features;
        if (guild.ApproximateMemberCount.HasValue)
            this.ApproximateMemberCount = guild.ApproximateMemberCount.Value;
        if (guild.ApproximatePresenceCount.HasValue)
            this.ApproximatePresenceCount = guild.ApproximatePresenceCount.Value;

        this.fetchedGuild ??= new FetchedGuildInfo(this);
        this.fetchedGuild.Update(guild);
    }

    internal void RefreshCurrentUser()
    {
        if (this.AccessClient.CurrentUser is null || this.CurrentUserSelf is not null)
            return;

        this.CurrentUserSelf = new OAuthGuildSelfUser(this, this.AccessClient.CurrentUser.GlobalUser);
        this.CurrentUserSelf.Update(this.tempCurrentUserInformation.ToModel(this));
    }

    /// <summary>
    /// Attempts to fetch this guild from the specified discord
    /// client.
    /// </summary>
    /// <param name="client">
    /// The client to fetch the guild from.
    /// </param>
    /// <param name="mode">
    /// The mode for caching.
    /// </param>
    /// <param name="options">
    /// The options for sending the request.
    /// </param>
    /// <returns>
    /// A task containing the result of whether or not the
    /// guild was fetched successfully.
    /// </returns>
    public Task<bool> TryFetchGuildFromClientAsync(IDiscordClient client, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null)
    {
        if (client is DiscordRestClient restClient)
            return this.TryFetchGuildFromClientAsync(restClient, mode, options);
        else if (client is BaseSocketClient socketClient)
            return this.TryFetchGuildFromClientAsync(socketClient, mode, options);
        else
            return this.TryFetchGuildFromUnknownClientAsync(client, mode, options);

    }

    /// <summary>
    /// Attempts to fetch this guild from the specified discord
    /// client.
    /// </summary>
    /// <param name="client">
    /// The client to fetch the guild from.
    /// </param>
    /// <param name="mode">
    /// The mode for caching.
    /// </param>
    /// <param name="options">
    /// The options for sending the request.
    /// </param>
    /// <returns>
    /// A task containing the result of whether or not the
    /// guild was fetched successfully.
    /// </returns>
    public async Task<bool> TryFetchGuildFromClientAsync(DiscordRestClient client, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null)
    {
        if (mode == CacheMode.CacheOnly)
            return false;

        Guild? model = await client.ApiClient.GetGuildAsync(this.Id, false, options).ConfigureAwait(false);
        if (model is null)
            return false;

        this.Update(model);
        return true;
    }

    /// <summary>
    /// Attempts to fetch this guild from the specified discord
    /// client.
    /// </summary>
    /// <param name="client">
    /// The client to fetch the guild from.
    /// </param>
    /// <param name="mode">
    /// The mode for caching.
    /// </param>
    /// <param name="options">
    /// The options for sending the request.
    /// </param>
    /// <returns>
    /// A task containing the result of whether or not the
    /// guild was fetched successfully.
    /// </returns>
    public async Task<bool> TryFetchGuildFromClientAsync(BaseSocketClient client, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null)
    {
        SocketGuild? guild = client.GetGuild(this.Id);
        if (guild is not null)
        {
            this.Update(guild);
            return true;
        }

        if (mode == CacheMode.CacheOnly)
            return false;

        Guild? model = await client.ApiClient.GetGuildAsync(this.Id, false, options).ConfigureAwait(false);
        if (model is null)
            return false;

        this.Update(model);
        return true;
    }

    private async Task<bool> TryFetchGuildFromUnknownClientAsync(IDiscordClient client, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null)
    {
        IGuild? guild = await client.GetGuildAsync(this.Id, mode, options).ConfigureAwait(false);
        if (guild is null)
            return false;
        this.Update(guild);
        return true;
    }

    #region IUserGuild
    bool IUserGuild.IsOwner => this.CurrentUserIsOwner;
    GuildPermissions IUserGuild.Permissions => this.CurrentUserPermissions;
    #endregion

    #region IGuild
    int IGuild.AFKTimeout => this.AFKTimeout.GetValueOrDefault(0);
    bool IGuild.IsWidgetEnabled => this.IsWidgetEnabled.GetValueOrDefault(false);
    DefaultMessageNotifications IGuild.DefaultMessageNotifications => this.DefaultMessageNotifications.GetValueOrDefault(Discord.DefaultMessageNotifications.AllMessages);
    MfaLevel IGuild.MfaLevel => this.MfaLevel.GetValueOrDefault(Discord.MfaLevel.Disabled);
    VerificationLevel IGuild.VerificationLevel => this.VerificationLevel.GetValueOrDefault(Discord.VerificationLevel.None);
    ExplicitContentFilterLevel IGuild.ExplicitContentFilter => this.ExplicitContentFilter.GetValueOrDefault(ExplicitContentFilterLevel.Disabled);
    string? IGuild.SplashId => this.SplashId.GetValueOrDefault(null);
    string? IGuild.SplashUrl => this.SplashUrl.GetValueOrDefault(null);
    string? IGuild.DiscoverySplashId => this.DiscoverySplashId.GetValueOrDefault(null);
    string? IGuild.DiscoverySplashUrl => this.DiscoverySplashUrl.GetValueOrDefault(null);
    bool IGuild.Available => false;
    ulong? IGuild.AFKChannelId => this.AFKChannelId.GetValueOrDefault(null);
    ulong? IGuild.WidgetChannelId => this.WidgetChannelId.GetValueOrDefault(null);
    ulong? IGuild.SafetyAlertsChannelId => this.SafetyAlertsChannelId.GetValueOrDefault(null);
    ulong? IGuild.SystemChannelId => this.SystemChannelId.GetValueOrDefault(null);
    ulong? IGuild.RulesChannelId => this.RulesChannelId.GetValueOrDefault(null);
    ulong? IGuild.PublicUpdatesChannelId => this.PublicUpdatesChannelId.GetValueOrDefault(null);
    ulong IGuild.OwnerId => this.OwnerId.GetValueOrDefault(this.CurrentUserIsOwner ? (this.CurrentUserSelf?.Id ?? 0UL) : 0);
    ulong? IGuild.ApplicationId => this.ApplicationId.GetValueOrDefault(null);
    string? IGuild.VoiceRegionId => null;
    IAudioClient? IGuild.AudioClient => null;
    IRole IGuild.EveryoneRole => this.EveryoneRole.GetValueOrDefault(this.everyoneRoleWrapper);
    IReadOnlyCollection<GuildEmote> IGuild.Emotes => this.Emotes.GetValueOrDefault(ImmutableArray<GuildEmote>.Empty);
    IReadOnlyCollection<ICustomSticker> IGuild.Stickers => this.Stickers.GetValueOrDefault(ImmutableArray<ICustomSticker>.Empty);
    IReadOnlyCollection<IRole> IGuild.Roles => this.Roles.GetValueOrDefault(ImmutableArray<IRole>.Empty);
    PremiumTier IGuild.PremiumTier => this.PremiumTier.GetValueOrDefault(Discord.PremiumTier.None);
    string? IGuild.BannerId => this.BannerId.GetValueOrDefault(null);
    string? IGuild.BannerUrl => this.BannerUrl.GetValueOrDefault(null);
    string? IGuild.VanityURLCode => this.VanityURLCode.GetValueOrDefault(null);
    SystemChannelMessageDeny IGuild.SystemChannelFlags => this.SystemChannelFlags.GetValueOrDefault(SystemChannelMessageDeny.None);
    string? IGuild.Description => this.Description.GetValueOrDefault(null);
    int IGuild.PremiumSubscriptionCount => this.PremiumSubscriptionCount.GetValueOrDefault(0);
    int? IGuild.MaxPresences => this.MaxPresences.GetValueOrDefault(null);
    int? IGuild.MaxMembers => this.MaxMembers.GetValueOrDefault(null);
    int? IGuild.MaxVideoChannelUsers => this.MaxVideoChannelUsers.GetValueOrDefault(null);
    int? IGuild.MaxStageVideoChannelUsers => this.MaxVideoChannelUsers.GetValueOrDefault(null);
    int IGuild.MaxBitrate => this.MaxBitrate.GetValueOrDefault(0);
    string IGuild.PreferredLocale => this.PreferredLocale.GetValueOrDefault(string.Empty);
    NsfwLevel IGuild.NsfwLevel => this.NsfwLevel.GetValueOrDefault(Discord.NsfwLevel.Default);
    CultureInfo IGuild.PreferredCulture => this.PreferredCulture.GetValueOrDefault(CultureInfo.InvariantCulture);
    bool IGuild.IsBoostProgressBarEnabled => this.IsBoostProgressBarEnabled.GetValueOrDefault(false);
    ulong IGuild.MaxUploadLimit => this.MaxUploadLimit.GetValueOrDefault(0);
    GuildInventorySettings? IGuild.InventorySettings => this.InventorySettings.GetValueOrDefault(null);
    GuildIncidentsData IGuild.IncidentsData => this.IncidentsData.GetValueOrDefault(new GuildIncidentsData());

    IRole? IGuild.GetRole(ulong id)
        => this.GetRole(id).GetValueOrDefault(null);

    Task IGuild.AddBanAsync(IUser user, int pruneDays, string? reason, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    Task IGuild.AddBanAsync(ulong userId, int pruneDays, string? reason, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    Task<IGuildUser?> IGuild.AddGuildUserAsync(ulong userId, string accessToken, Action<AddGuildUserProperties>? func, RequestOptions? options)
        => Task.FromException<IGuildUser?>(new NotSupportedException());

    Task<IReadOnlyCollection<IApplicationCommand>> IGuild.BulkOverwriteApplicationCommandsAsync(ApplicationCommandProperties[] properties, RequestOptions? options)
        => Task.FromException<IReadOnlyCollection<IApplicationCommand>>(new NotSupportedException());

    Task<IApplicationCommand> IGuild.CreateApplicationCommandAsync(ApplicationCommandProperties properties, RequestOptions? options)
        => Task.FromException<IApplicationCommand>(new NotSupportedException());

    Task<IAutoModRule> IGuild.CreateAutoModRuleAsync(Action<AutoModRuleProperties> props, RequestOptions? options)
        => Task.FromException<IAutoModRule>(new NotSupportedException());

    Task<ICategoryChannel> IGuild.CreateCategoryAsync(string name, Action<GuildChannelProperties>? func, RequestOptions? options)
        => Task.FromException<ICategoryChannel>(new NotSupportedException());

    Task<GuildEmote> IGuild.CreateEmoteAsync(string name, Image image, Optional<IEnumerable<IRole>?> roles, RequestOptions? options)
        => Task.FromException<GuildEmote>(new NotSupportedException());

    Task<IGuildScheduledEvent> IGuild.CreateEventAsync(string name, DateTimeOffset startTime, GuildScheduledEventType type, GuildScheduledEventPrivacyLevel privacyLevel, string? description, DateTimeOffset? endTime, ulong? channelId, string? location, Image? coverImage, RequestOptions? options)
        => Task.FromException<IGuildScheduledEvent>(new NotSupportedException());

    Task<IForumChannel> IGuild.CreateForumChannelAsync(string name, Action<ForumChannelProperties>? func, RequestOptions? options)
        => Task.FromException<IForumChannel>(new NotSupportedException());

    Task<IMediaChannel> IGuild.CreateMediaChannelAsync(string name, Action<ForumChannelProperties>? func, RequestOptions? options)
        => Task.FromException<IMediaChannel>(new NotSupportedException());

    Task<IRole> IGuild.CreateRoleAsync(string name, GuildPermissions? permissions, Color? color, bool isHoisted, RequestOptions? options)
        => Task.FromException<IRole>(new NotSupportedException());

    Task<IRole> IGuild.CreateRoleAsync(string name, GuildPermissions? permissions, Color? color, bool isHoisted, bool isMentionable, RequestOptions? options, Image? icon, Emoji? emoji)
        => Task.FromException<IRole>(new NotSupportedException());

    Task<IStageChannel> IGuild.CreateStageChannelAsync(string name, Action<VoiceChannelProperties>? func, RequestOptions? options)
        => Task.FromException<IStageChannel>(new NotSupportedException());

    Task<ICustomSticker> IGuild.CreateStickerAsync(string name, Image image, IEnumerable<string> tags, string? description, RequestOptions? options)
        => Task.FromException<ICustomSticker>(new NotSupportedException());

    Task<ICustomSticker> IGuild.CreateStickerAsync(string name, string path, IEnumerable<string> tags, string? description, RequestOptions? options)
        => Task.FromException<ICustomSticker>(new NotSupportedException());

    Task<ICustomSticker> IGuild.CreateStickerAsync(string name, Stream stream, string filename, IEnumerable<string> tags, string? description, RequestOptions? options)
        => Task.FromException<ICustomSticker>(new NotSupportedException());

    Task<ITextChannel> IGuild.CreateTextChannelAsync(string name, Action<TextChannelProperties>? func, RequestOptions? options)
        => Task.FromException<ITextChannel>(new NotSupportedException());

    Task<IVoiceChannel> IGuild.CreateVoiceChannelAsync(string name, Action<VoiceChannelProperties>? func, RequestOptions? options)
        => Task.FromException<IVoiceChannel>(new NotSupportedException());

    Task IGuild.DeleteEmoteAsync(GuildEmote emote, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    Task IGuild.DeleteIntegrationAsync(ulong id, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    Task IGuild.DeleteStickerAsync(ICustomSticker sticker, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    Task IGuild.DisconnectAsync(IGuildUser user)
        => Task.FromException(new NotSupportedException());

    Task IGuild.DownloadUsersAsync()
        => Task.FromException(new NotSupportedException());

    Task<IVoiceChannel?> IGuild.GetAFKChannelAsync(CacheMode mode, RequestOptions? options)
        => Task.FromException<IVoiceChannel?>(new NotSupportedException());

    Task<IApplicationCommand?> IGuild.GetApplicationCommandAsync(ulong id, CacheMode mode, RequestOptions? options)
        => Task.FromException<IApplicationCommand?>(new NotSupportedException());

    Task<IReadOnlyCollection<IApplicationCommand>> IGuild.GetApplicationCommandsAsync(bool withLocalizations, string? locale, RequestOptions? options)
        => Task.FromException<IReadOnlyCollection<IApplicationCommand>>(new NotSupportedException());

    Task<IReadOnlyCollection<IAuditLogEntry>> IGuild.GetAuditLogsAsync(int limit, CacheMode mode, RequestOptions? options, ulong? beforeId, ulong? userId, ActionType? actionType, ulong? afterId)
        => Task.FromException<IReadOnlyCollection<IAuditLogEntry>>(new NotSupportedException());

    Task<IAutoModRule?> IGuild.GetAutoModRuleAsync(ulong ruleId, RequestOptions? options)
        => Task.FromException<IAutoModRule?>(new NotSupportedException());

    Task<IAutoModRule[]> IGuild.GetAutoModRulesAsync(RequestOptions? options)
        => Task.FromException<IAutoModRule[]>(new NotSupportedException());

    Task<IBan?> IGuild.GetBanAsync(IUser user, RequestOptions? options)
        => Task.FromException<IBan?>(new NotSupportedException());

    Task<IBan?> IGuild.GetBanAsync(ulong userId, RequestOptions? options)
        => Task.FromException<IBan?>(new NotSupportedException());

    IAsyncEnumerable<IReadOnlyCollection<IBan>> IGuild.GetBansAsync(int limit, RequestOptions? options)
        => throw new NotSupportedException();

    IAsyncEnumerable<IReadOnlyCollection<IBan>> IGuild.GetBansAsync(ulong fromUserId, Direction dir, int limit, RequestOptions? options)
        => throw new NotSupportedException();

    IAsyncEnumerable<IReadOnlyCollection<IBan>> IGuild.GetBansAsync(IUser fromUser, Direction dir, int limit, RequestOptions? options)
        => throw new NotSupportedException();

    Task<IReadOnlyCollection<ICategoryChannel>> IGuild.GetCategoriesAsync(CacheMode mode, RequestOptions? options)
        => Task.FromException<IReadOnlyCollection<ICategoryChannel>>(new NotSupportedException());

    Task<IGuildChannel?> IGuild.GetChannelAsync(ulong id, CacheMode mode, RequestOptions? options)
        => Task.FromException<IGuildChannel?>(new NotSupportedException());

    Task<IReadOnlyCollection<IGuildChannel>> IGuild.GetChannelsAsync(CacheMode mode, RequestOptions? options)
        => Task.FromException<IReadOnlyCollection<IGuildChannel>>(new NotSupportedException());

    async Task<IGuildUser> IGuild.GetCurrentUserAsync(CacheMode mode, RequestOptions? options)
    {
        if (mode == CacheMode.CacheOnly)
        {
            return this.CurrentUserSelf ?? throw new KeyNotFoundException();
        }

        await this.AccessClient.GetMyCurrentUserAsync(options).ConfigureAwait(false);
        return this.CurrentUserSelf!;
    }

    Task<ITextChannel?> IGuild.GetDefaultChannelAsync(CacheMode mode, RequestOptions? options)
        => Task.FromException<ITextChannel?>(new NotSupportedException());

    Task<GuildEmote?> IGuild.GetEmoteAsync(ulong id, RequestOptions? options)
        => Task.FromException<GuildEmote?>(new NotSupportedException());

    Task<IReadOnlyCollection<GuildEmote>> IGuild.GetEmotesAsync(RequestOptions? options)
        => Task.FromException<IReadOnlyCollection<GuildEmote>>(new NotSupportedException());

    Task<IGuildScheduledEvent?> IGuild.GetEventAsync(ulong id, RequestOptions? options)
        => Task.FromException<IGuildScheduledEvent?>(new NotSupportedException());

    Task<IReadOnlyCollection<IGuildScheduledEvent>> IGuild.GetEventsAsync(RequestOptions? options)
        => Task.FromException<IReadOnlyCollection<IGuildScheduledEvent>>(new NotSupportedException());

    Task<IForumChannel?> IGuild.GetForumChannelAsync(ulong id, CacheMode mode, RequestOptions? options)
        => Task.FromException<IForumChannel?>(new NotSupportedException());

    Task<IReadOnlyCollection<IForumChannel>> IGuild.GetForumChannelsAsync(CacheMode mode, RequestOptions? options)
        => Task.FromException<IReadOnlyCollection<IForumChannel>>(new NotSupportedException());

    Task<IReadOnlyCollection<IIntegration>> IGuild.GetIntegrationsAsync(RequestOptions? options)
        => Task.FromException<IReadOnlyCollection<IIntegration>>(new NotSupportedException());

    Task<IReadOnlyCollection<IInviteMetadata>> IGuild.GetInvitesAsync(RequestOptions? options)
        => Task.FromException<IReadOnlyCollection<IInviteMetadata>>(new NotSupportedException());

    Task<IMediaChannel?> IGuild.GetMediaChannelAsync(ulong id, CacheMode mode, RequestOptions? options)
        => Task.FromException<IMediaChannel?>(new NotSupportedException());

    Task<IReadOnlyCollection<IMediaChannel>> IGuild.GetMediaChannelsAsync(CacheMode mode, RequestOptions? options)
        => Task.FromException<IReadOnlyCollection<IMediaChannel>>(new NotSupportedException());

    Task<IGuildOnboarding> IGuild.GetOnboardingAsync(RequestOptions? options)
        => Task.FromException<IGuildOnboarding>(new NotSupportedException());

    Task<IGuildUser?> IGuild.GetOwnerAsync(CacheMode mode, RequestOptions? options)
        => Task.FromException<IGuildUser?>(new NotSupportedException());

    Task<ITextChannel?> IGuild.GetPublicUpdatesChannelAsync(CacheMode mode, RequestOptions? options)
        => Task.FromException<ITextChannel?>(new NotSupportedException());

    Task<ITextChannel?> IGuild.GetRulesChannelAsync(CacheMode mode, RequestOptions? options)
        => Task.FromException<ITextChannel?>(new NotSupportedException());

    Task<IStageChannel?> IGuild.GetStageChannelAsync(ulong id, CacheMode mode, RequestOptions? options)
        => Task.FromException<IStageChannel?>(new NotSupportedException());

    Task<IReadOnlyCollection<IStageChannel>> IGuild.GetStageChannelsAsync(CacheMode mode, RequestOptions? options)
        => Task.FromException<IReadOnlyCollection<IStageChannel>>(new NotSupportedException());

    Task<ICustomSticker?> IGuild.GetStickerAsync(ulong id, CacheMode mode, RequestOptions? options)
        => Task.FromException<ICustomSticker?>(new NotSupportedException());

    Task<IReadOnlyCollection<ICustomSticker>> IGuild.GetStickersAsync(CacheMode mode, RequestOptions? options)
        => Task.FromException<IReadOnlyCollection<ICustomSticker>>(new NotSupportedException());

    Task<ITextChannel?> IGuild.GetSystemChannelAsync(CacheMode mode, RequestOptions? options)
        => Task.FromException<ITextChannel?>(new NotSupportedException());

    Task<ITextChannel?> IGuild.GetTextChannelAsync(ulong id, CacheMode mode, RequestOptions? options)
        => Task.FromException<ITextChannel?>(new NotSupportedException());

    Task<IReadOnlyCollection<ITextChannel>> IGuild.GetTextChannelsAsync(CacheMode mode, RequestOptions? options)
        => Task.FromException<IReadOnlyCollection<ITextChannel>>(new NotSupportedException());

    Task<IThreadChannel?> IGuild.GetThreadChannelAsync(ulong id, CacheMode mode, RequestOptions? options)
        => Task.FromException<IThreadChannel?>(new NotSupportedException());

    Task<IReadOnlyCollection<IThreadChannel>> IGuild.GetThreadChannelsAsync(CacheMode mode, RequestOptions? options)
        => Task.FromException<IReadOnlyCollection<IThreadChannel>>(new NotSupportedException());

    Task<IGuildUser?> IGuild.GetUserAsync(ulong id, CacheMode mode, RequestOptions? options)
        => Task.FromException<IGuildUser?>(new NotSupportedException());

    Task<IReadOnlyCollection<IGuildUser>> IGuild.GetUsersAsync(CacheMode mode, RequestOptions? options)
        => Task.FromException<IReadOnlyCollection<IGuildUser>>(new NotSupportedException());

    Task<IInviteMetadata?> IGuild.GetVanityInviteAsync(RequestOptions? options)
        => Task.FromException<IInviteMetadata?>(new NotSupportedException());

    Task<IVoiceChannel?> IGuild.GetVoiceChannelAsync(ulong id, CacheMode mode, RequestOptions? options)
        => Task.FromException<IVoiceChannel?>(new NotSupportedException());

    Task<IReadOnlyCollection<IVoiceChannel>> IGuild.GetVoiceChannelsAsync(CacheMode mode, RequestOptions? options)
        => Task.FromException<IReadOnlyCollection<IVoiceChannel>>(new NotSupportedException());

    Task<IReadOnlyCollection<IVoiceRegion>> IGuild.GetVoiceRegionsAsync(RequestOptions? options)
        => Task.FromException<IReadOnlyCollection<IVoiceRegion>>(new NotSupportedException());

    Task<IWebhook?> IGuild.GetWebhookAsync(ulong id, RequestOptions? options)
        => Task.FromException<IWebhook?>(new NotSupportedException());

    Task<IReadOnlyCollection<IWebhook>> IGuild.GetWebhooksAsync(RequestOptions? options)
        => Task.FromException<IReadOnlyCollection<IWebhook>>(new NotSupportedException());

    Task<WelcomeScreen?> IGuild.GetWelcomeScreenAsync(RequestOptions? options)
        => Task.FromException<WelcomeScreen?>(new NotSupportedException());

    Task<IGuildChannel?> IGuild.GetWidgetChannelAsync(CacheMode mode, RequestOptions? options)
        => Task.FromException<IGuildChannel?>(new NotSupportedException());

    Task IGuild.LeaveAsync(RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    Task IGuild.ModifyAsync(Action<GuildProperties> func, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    Task<GuildEmote> IGuild.ModifyEmoteAsync(GuildEmote emote, Action<EmoteProperties> func, RequestOptions? options)
        => Task.FromException<GuildEmote>(new NotSupportedException());

    Task<GuildIncidentsData> IGuild.ModifyIncidentActionsAsync(Action<GuildIncidentsDataProperties> props, RequestOptions? options)
        => Task.FromException<GuildIncidentsData>(new NotSupportedException());

    Task<IGuildOnboarding> IGuild.ModifyOnboardingAsync(Action<GuildOnboardingProperties> props, RequestOptions? options)
        => Task.FromException<IGuildOnboarding>(new NotSupportedException());

    Task<WelcomeScreen?> IGuild.ModifyWelcomeScreenAsync(bool enabled, WelcomeScreenChannelProperties[] channels, string? description, RequestOptions? options)
        => Task.FromException<WelcomeScreen?>(new NotSupportedException());

    Task IGuild.ModifyWidgetAsync(Action<GuildWidgetProperties> func, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    Task IGuild.MoveAsync(IGuildUser user, IVoiceChannel targetChannel)
        => Task.FromException(new NotSupportedException());

    Task<int> IGuild.PruneUsersAsync(int days, bool simulate, RequestOptions? options, IEnumerable<ulong>? includeRoleIds)
        => Task.FromException<int>(new NotSupportedException());

    Task IGuild.RemoveBanAsync(IUser user, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    Task IGuild.RemoveBanAsync(ulong userId, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    Task IGuild.ReorderChannelsAsync(IEnumerable<ReorderChannelProperties> args, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    Task IGuild.ReorderRolesAsync(IEnumerable<ReorderRoleProperties> args, RequestOptions? options)
        => Task.FromException(new NotSupportedException());

    Task<IReadOnlyCollection<IGuildUser>> IGuild.SearchUsersAsync(string query, int limit, CacheMode mode, RequestOptions? options)
        => Task.FromException<IReadOnlyCollection<IGuildUser>>(new NotSupportedException());
    #endregion

    #region IDeletable
    Task IDeletable.DeleteAsync(RequestOptions? options)
        => Task.FromException(new NotSupportedException());
    #endregion

    private sealed class EveryoneRoleWrapper : IRole
    {
        public EveryoneRoleWrapper(OAuthGuild guild)
        {
            this.Guild = guild;
            this.Id = guild.Id;
        }

        public ulong Id { get; }
        public DateTimeOffset CreatedAt => SnowflakeUtils.FromSnowflake(this.Id);
        public IGuild Guild { get; }
        public Color Color => default;
        public bool IsHoisted => false;
        public bool IsManaged => false;
        public bool IsMentionable => false;
        public string Name => "@everyone";
        public string? Icon => null;
        public Emoji? Emoji => null;
        public GuildPermissions Permissions => GuildPermissions.None;
        public int Position => 0;
        public RoleTags? Tags => null;
        public RoleFlags Flags => RoleFlags.None;
        public string Mention => "@everyone";
        public int CompareTo(IRole? other)
        {
            if (other is null)
                return 1;
            
            int result = this.Position.CompareTo(other.Position);
            if (result != 0) return result;
            else return this.Id.CompareTo(other.Id);
        }

        public Task DeleteAsync(RequestOptions? options = null)
            => Task.FromException(new NotSupportedException());

        public string? GetIconUrl()
            => null;

        public Task ModifyAsync(Action<RoleProperties> func, RequestOptions? options = null)
            => Task.FromException(new NotSupportedException());
    }

    [DebuggerDisplay(@"{DebuggerDisplay,nq}")]
    private sealed class FetchedRoleInfo : IRole
    {
        public FetchedRoleInfo(FetchedGuildInfo guild, ulong id)
        {
            this.FetchedGuild = guild;
            this.Id = id;
            this.IsEveryone = id == guild.Guild.Id;
            this.Name = string.Empty;
        }

        public FetchedGuildInfo FetchedGuild { get; }
        IGuild IRole.Guild => this.FetchedGuild.Guild;

        public ulong Id { get; set; }
        public DateTimeOffset CreatedAt => SnowflakeUtils.FromSnowflake(this.Id);
        public Color Color { get; set; }
        public bool IsHoisted { get; set; }
        public bool IsManaged { get; set; }
        public bool IsMentionable { get; set; }
        public string Name { get; set; }
        public string? Icon { get; set; }
        public Emoji? Emoji { get; set; }
        public GuildPermissions Permissions { get; set; }
        public int Position { get; set; }
        public RoleTags? Tags { get; set; }
        public RoleFlags Flags { get; set; }
        public bool IsEveryone { get; set; }

        public string Mention => this.IsEveryone ?
            "@everyone" :
            MentionUtils.MentionRole(this.Id);

        public string? GetIconUrl()
            => CDN.GetGuildRoleIconUrl(this.Id, this.Icon);

        public override string ToString() => this.Name;
        private string DebuggerDisplay => $"{this.Name} ({this.Id})";

        public int CompareTo(IRole? other)
        {
            return RoleUtils.Compare(this, other);
        }

        public Task ModifyAsync(Action<RoleProperties> func, RequestOptions? options)
        {
            return Task.FromException(new NotSupportedException());
        }

        public Task DeleteAsync(RequestOptions? options)
        {
            return Task.FromException(new NotSupportedException());
        }

        internal void Update(Role model)
        {
            this.Color = new Color(model.Color);
            this.IsHoisted = model.Hoist;
            this.IsManaged = model.Managed;
            this.IsMentionable = model.Mentionable;
            this.Name = model.Name;
            this.Permissions = new GuildPermissions(model.Permissions);
            this.Position = model.Position;
            this.Flags = model.Flags;
            if (model.Icon.IsSpecified)
                this.Icon = model.Icon.Value;
            if (model.Emoji.IsSpecified)
                this.Emoji = new Emoji(model.Emoji.Value);
            if (model.Tags.IsSpecified)
                this.Tags = model.Tags.Value.ToEntity();
        }

        internal void Update(SocketRole role)
        {
            this.Color = role.Color;
            this.IsHoisted = role.IsHoisted;
            this.IsManaged = role.IsManaged;
            this.IsMentionable = role.IsMentionable;
            this.Name = role.Name;
            this.Icon = role.Icon;
            this.Emoji = role.Emoji;
            this.Permissions = role.Permissions;
            this.Position = role.Position;
            this.Tags = role.Tags;
            this.Flags = role.Flags;
        }

        internal void Update(RestRole role)
        {
            this.Color = role.Color;
            this.IsHoisted = role.IsHoisted;
            this.IsManaged = role.IsManaged;
            this.IsMentionable = role.IsMentionable;
            this.Name = role.Name;
            this.Icon = role.Icon;
            this.Emoji = role.Emoji;
            this.Permissions = role.Permissions;
            this.Position = role.Position;
            this.Tags = role.Tags;
            this.Flags = role.Flags;
        }

        internal void Update(IRole role)
        {
            this.Color = role.Color;
            this.IsHoisted = role.IsHoisted;
            this.IsManaged = role.IsManaged;
            this.IsMentionable = role.IsMentionable;
            this.Name = role.Name;
            this.Icon = role.Icon;
            this.Emoji = role.Emoji;
            this.Permissions = role.Permissions;
            this.Position = role.Position;
            this.Tags = role.Tags;
            this.Flags = role.Flags;
        }
    }

    [DebuggerDisplay(@"{DebuggerDisplay,nq}")]
    private sealed class FetchedCustomStickerInfo : ICustomSticker
    {
        public FetchedCustomStickerInfo(FetchedGuildInfo guild, ulong id)
        {
            this.Id = id;
            this.FetchedGuild = guild;
            this.GuildId = guild.Guild.Id;
            this.Description = string.Empty;
            this.Name = string.Empty;
            this.Tags = Array.Empty<string>();
        }

        public ulong Id { get; }
        public DateTimeOffset CreatedAt => SnowflakeUtils.FromSnowflake(this.Id);
        public ulong PackId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IReadOnlyCollection<string> Tags { get; set; }
        public StickerType Type { get; set; }
        public bool? IsAvailable { get; set; }
        public int? SortOrder { get; set; }
        public StickerFormatType Format { get; set; }
        public ulong? AuthorId { get; set; }
        public FetchedGuildInfo FetchedGuild { get; }
        public ulong GuildId { get; }
        IGuild ICustomSticker.Guild => this.FetchedGuild.Guild;

        public string GetStickerUrl()
            => CDN.GetStickerUrl(this.Id, this.Format);

        private string DebuggerDisplay
        {
            get
            {
                if (this.FetchedGuild is null)
                    return $"{this.Name} ({this.Id})";
                else
                    return $"{this.Name} in {this.FetchedGuild.Guild.Name} ({this.Id})";
            }
        }

        public Task DeleteAsync(RequestOptions? options)
        {
            return Task.FromException(new NotSupportedException());
        }

        public Task ModifyAsync(Action<StickerProperties> func, RequestOptions? options)
        {
            return Task.FromException(new NotSupportedException());
        }

        internal void Update(Discord.API.Sticker model)
        {
            this.PackId = model.PackId;
            this.Name = model.Name;
            this.Description = model.Description;
            this.Type = model.Type;
            this.IsAvailable = model.Available;
            this.SortOrder = model.SortValue;
            this.Format = model.FormatType;
            if (model.Tags.IsSpecified)
                this.Tags = model.Tags.Value
                    .Split(',')
                    .Select(x => x.Trim())
                    .ToArray();
            if (model.User.IsSpecified)
                this.AuthorId = model.User.Value.Id;
        }

        internal void Update(CustomSticker sticker)
        {
            this.PackId = sticker.PackId;
            this.Name = sticker.Name;
            this.Description = sticker.Description;
            this.Tags = sticker.Tags;
            this.Type = sticker.Type;
            this.IsAvailable = sticker.IsAvailable;
            this.SortOrder = sticker.SortOrder;
            this.Format = sticker.Format;
            this.AuthorId = sticker.AuthorId;
        }

        internal void Update(SocketCustomSticker sticker)
        {
            this.PackId = sticker.PackId;
            this.Name = sticker.Name;
            this.Description = sticker.Description;
            this.Tags = sticker.Tags;
            this.Type = sticker.Type;
            this.IsAvailable = sticker.IsAvailable;
            this.SortOrder = sticker.SortOrder;
            this.Format = sticker.Format;
            this.AuthorId = sticker.AuthorId;
        }

        internal void Update(ICustomSticker sticker)
        {
            this.PackId = sticker.PackId;
            this.Name = sticker.Name;
            this.Description = sticker.Description;
            this.Tags = sticker.Tags;
            this.Type = sticker.Type;
            this.IsAvailable = sticker.IsAvailable;
            this.SortOrder = sticker.SortOrder;
            this.Format = sticker.Format;
            this.AuthorId = sticker.AuthorId;
        }
    }

    private sealed class FetchedGuildInfo
    {
        private ImmutableArray<GuildEmote> emotes;
        private ImmutableArray<FetchedCustomStickerInfo> stickers;
        private ImmutableDictionary<ulong, FetchedRoleInfo> roles;
        private FetchedRoleInfo everyoneRole;

        public FetchedGuildInfo(OAuthGuild guild)
        {
            this.emotes = ImmutableArray<GuildEmote>.Empty;
            this.stickers = ImmutableArray<FetchedCustomStickerInfo>.Empty;
            this.roles = ImmutableDictionary<ulong, FetchedRoleInfo>.Empty;
            this.everyoneRole = null!;

            this.Guild = guild;
            this.PreferredLocale = string.Empty;
            this.PreferredCulture = CultureInfo.InvariantCulture;
            this.IncidentsData = null!;
        }

        public OAuthGuild Guild { get; }

        public int AfkTimeout { get; set; }
        public bool IsWidgetEnabled { get; set; }
        public DefaultMessageNotifications DefaultMessageNotifications { get; set; }
        public MfaLevel MfaLevel { get; set; }
        public VerificationLevel VerificationLevel { get; set; }
        public ExplicitContentFilterLevel ExplicitContentFilter { get; set; }
        public string? SplashId { get; set; }
        public string? SplashUrl => CDN.GetGuildSplashUrl(this.Guild.Id, this.SplashId);
        public string? DiscoverySplashId { get; set; }
        public string? DiscoverySplashUrl => CDN.GetGuildDiscoverySplashUrl(this.Guild.Id, this.DiscoverySplashId);
        public ulong? AFKChannelId { get; set; }
        public ulong? WidgetChannelId { get; set; }
        public ulong? SafetyAlertsChannelId { get; set; }
        public ulong? SystemChannelId { get; set; }
        public ulong? RulesChannelId { get; set; }
        public ulong? PublicUpdatesChannelId { get; set; }
        public ulong OwnerId { get; set; }
        public ulong? ApplicationId { get; set; }
        public IRole EveryoneRole => this.everyoneRole;
        public IReadOnlyCollection<GuildEmote> Emotes => this.emotes;
        public IReadOnlyCollection<ICustomSticker> Stickers => this.stickers;
        public IReadOnlyCollection<IRole> Roles => this.roles.ToReadOnlyCollection();
        public PremiumTier PremiumTier { get; set; }
        public string? BannerId { get; set; }
        public string? BannerUrl => CDN.GetGuildBannerUrl(this.Guild.Id, this.BannerId, ImageFormat.Auto);
        public string? VanityURLCode { get; set; }
        public SystemChannelMessageDeny SystemChannelFlags { get; set; }
        public string? Description { get; set; }
        public int PremiumSubscriptionCount { get; set; }
        public int? MaxPresences { get; set; }
        public int? MaxMembers { get; set; }
        public int? MaxVideoChannelUsers { get; set; }
        public int? MaxStageVideoChannelUsers { get; set; }
        public int MaxBitrate => GuildHelper.GetMaxBitrate(this.PremiumTier);
        public string PreferredLocale { get; set; }
        public NsfwLevel NsfwLevel  { get; set; }
        public CultureInfo PreferredCulture { get; set; }
        public bool IsBoostProgressBarEnabled { get; set; }
        public ulong MaxUploadLimit => GuildHelper.GetUploadLimit(this.PremiumTier);
        public GuildInventorySettings? InventorySettings { get; set; }
        public GuildIncidentsData IncidentsData { get; set; }

        public IRole? GetRole(ulong id)
        {
            if (this.roles.TryGetValue(id, out FetchedRoleInfo? role))
                return role;
            return null;
        }

        internal void Update(Guild model)
        {
            this.AfkTimeout = model.AFKTimeout;
            this.DefaultMessageNotifications = model.DefaultMessageNotifications;
            this.MfaLevel = model.MfaLevel;
            this.VerificationLevel = model.VerificationLevel;
            this.ExplicitContentFilter = model.ExplicitContentFilter;
            this.SplashId = model.Splash;
            this.DiscoverySplashId = model.DiscoverySplash;
            this.AFKChannelId = model.AFKChannelId;
            this.SystemChannelId = model.SystemChannelId;
            this.RulesChannelId = model.RulesChannelId;
            this.PublicUpdatesChannelId = model.PublicUpdatesChannelId;
            this.OwnerId = model.OwnerId;
            this.ApplicationId = model.ApplicationId;
            this.PremiumTier = model.PremiumTier;
            this.BannerId = model.Banner;
            this.VanityURLCode = model.VanityURLCode;
            this.SystemChannelFlags = model.SystemChannelFlags;
            this.Description = model.Description;
            this.PremiumSubscriptionCount = model.PremiumSubscriptionCount.GetValueOrDefault();
            this.PreferredLocale = model.PreferredLocale;
            this.PreferredCulture = new CultureInfo(this.PreferredLocale);
            this.IsBoostProgressBarEnabled = model.IsBoostProgressBarEnabled.GetValueOrDefault();

            if (model.Roles is not null)
            {
                ImmutableDictionary<ulong, FetchedRoleInfo>.Builder roles = ImmutableDictionary.CreateBuilder<ulong, FetchedRoleInfo>();
                foreach (Role roleModel in model.Roles)
                {
                    if (!this.roles.TryGetValue(roleModel.Id, out FetchedRoleInfo? role))
                    {
                        role = new FetchedRoleInfo(this, roleModel.Id);
                    }

                    role.Update(roleModel);
                    roles.Add(roleModel.Id, role);
                }
                this.roles = roles.ToImmutable();
                this.everyoneRole = this.roles[this.Guild.Id];
            }

            if (model.Emojis is not null)
            {
                if (model.Emojis.Length > 0)
                {
                    ImmutableArray<GuildEmote>.Builder emotes = ImmutableArray.CreateBuilder<GuildEmote>(model.Emojis.Length);
                    foreach (Discord.API.Emoji emoteModel in model.Emojis)
                    {
                        emotes.Add(emoteModel.ToEntity());
                    }
                    this.emotes = emotes.ToImmutable();
                }
                else
                    this.emotes = ImmutableArray<GuildEmote>.Empty;
            }

            if (model.Stickers is not null)
            {
                if (model.Stickers.Length > 0)
                {
                    ImmutableArray<FetchedCustomStickerInfo>.Builder stickers = ImmutableArray.CreateBuilder<FetchedCustomStickerInfo>(model.Stickers.Length);
                    foreach (Discord.API.Sticker stickerModel in model.Stickers)
                    {
                        FetchedCustomStickerInfo sticker = this.stickers.FirstOrDefault(x => x.Id == stickerModel.Id) ?? new FetchedCustomStickerInfo(this, stickerModel.Id);

                        sticker.Update(stickerModel);
                        stickers.Add(sticker);
                    }
                    this.stickers = stickers.ToImmutable();
                }
                else
                    this.stickers = ImmutableArray<FetchedCustomStickerInfo>.Empty;
            }

            if (model.WidgetEnabled.IsSpecified)
                this.IsWidgetEnabled = model.WidgetEnabled.Value;
            if (model.WidgetChannelId.IsSpecified)
                this.WidgetChannelId = model.WidgetChannelId.Value;
            if (model.SafetyAlertsChannelId.IsSpecified)
                this.SafetyAlertsChannelId = model.SafetyAlertsChannelId.Value;
            if (model.MaxPresences.IsSpecified)
                this.MaxPresences = model.MaxPresences.Value ?? 25000;
            if (model.MaxPresences.IsSpecified)
                this.MaxMembers = model.MaxMembers.Value;
            if (model.MaxVideoChannelUsers.IsSpecified)
                this.MaxVideoChannelUsers = model.MaxVideoChannelUsers.Value;
            if (model.MaxStageVideoChannelUsers.IsSpecified)
                this.MaxStageVideoChannelUsers = model.MaxStageVideoChannelUsers.Value;
            if (model.InventorySettings.IsSpecified)
            {
                if (model.InventorySettings.Value is null)
                    this.InventorySettings = null;
                else
                    this.InventorySettings = new GuildInventorySettings(model.InventorySettings.Value.IsEmojiPackCollectible.GetValueOrDefault(false));
            }
            this.IncidentsData = model.IncidentsData is null ?
                new GuildIncidentsData() :
                new GuildIncidentsData()
                {
                    DmsDisabledUntil = model.IncidentsData.DmsDisabledUntil,
                    InvitesDisabledUntil = model.IncidentsData.InvitesDisabledUntil
                };
        }

        internal void Update(SocketGuild guild)
        {
            this.AfkTimeout = guild.AFKTimeout;
            this.IsWidgetEnabled = guild.IsWidgetEnabled;
            this.DefaultMessageNotifications = guild.DefaultMessageNotifications;
            this.MfaLevel = guild.MfaLevel;
            this.VerificationLevel = guild.VerificationLevel;
            this.ExplicitContentFilter = guild.ExplicitContentFilter;
            this.SplashId = guild.SplashId;
            this.DiscoverySplashId = guild.DiscoverySplashId;
            this.AFKChannelId = guild.AFKChannel?.Id;
            this.WidgetChannelId = guild.WidgetChannel?.Id;
            this.SafetyAlertsChannelId = guild.SafetyAlertsChannel?.Id;
            this.SystemChannelId = guild.SystemChannel?.Id;
            this.RulesChannelId = guild.RulesChannel?.Id;
            this.PublicUpdatesChannelId = guild.PublicUpdatesChannel?.Id;
            this.OwnerId = guild.OwnerId;
            this.ApplicationId = guild.ApplicationId;
            this.PremiumTier = guild.PremiumTier;
            this.BannerId = guild.BannerId;
            this.VanityURLCode = guild.VanityURLCode;
            this.SystemChannelFlags = guild.SystemChannelFlags;
            this.Description = guild.Description;
            this.PremiumSubscriptionCount = guild.PremiumSubscriptionCount;
            this.MaxPresences = guild.MaxPresences;
            this.MaxMembers = guild.MaxMembers;
            this.MaxVideoChannelUsers = guild.MaxVideoChannelUsers;
            this.MaxStageVideoChannelUsers = guild.MaxStageVideoChannelUsers;
            this.PreferredLocale = guild.PreferredLocale;
            this.PreferredCulture = guild.PreferredCulture;
            this.IsBoostProgressBarEnabled = guild.IsBoostProgressBarEnabled;
            this.InventorySettings = guild.InventorySettings;
            this.IncidentsData = guild.IncidentsData;

            IReadOnlyCollection<SocketRole> guildRoles = guild.Roles;
            ImmutableDictionary<ulong, FetchedRoleInfo>.Builder roles = ImmutableDictionary.CreateBuilder<ulong, FetchedRoleInfo>();
            foreach (SocketRole guildRole in guildRoles)
            {
                if (!this.roles.TryGetValue(guildRole.Id, out FetchedRoleInfo? role))
                {
                    role = new FetchedRoleInfo(this, guildRole.Id);
                }

                role.Update(guildRole);
                roles.Add(guildRole.Id, role);
            }
            this.roles = roles.ToImmutable();
            this.everyoneRole = this.roles[this.Guild.Id];
            this.emotes = guild.Emotes.ToImmutableArray();

            IReadOnlyCollection<SocketCustomSticker> guildStickers = guild.Stickers;

            if (guildStickers.Count > 0)
            {
                ImmutableArray<FetchedCustomStickerInfo>.Builder stickers = ImmutableArray.CreateBuilder<FetchedCustomStickerInfo>(guildStickers.Count);
                foreach (SocketCustomSticker guildSticker in guildStickers)
                {
                    FetchedCustomStickerInfo sticker = this.stickers.FirstOrDefault(x => x.Id == guildSticker.Id) ?? new FetchedCustomStickerInfo(this, guildSticker.Id);

                    sticker.Update(guildSticker);
                    stickers.Add(sticker);
                }
                this.stickers = stickers.ToImmutable();
            }
            else
                this.stickers = ImmutableArray<FetchedCustomStickerInfo>.Empty;
        }

        internal void Update(RestGuild guild)
        {
            this.AfkTimeout = guild.AFKTimeout;
            this.IsWidgetEnabled = guild.IsWidgetEnabled;
            this.DefaultMessageNotifications = guild.DefaultMessageNotifications;
            this.MfaLevel = guild.MfaLevel;
            this.VerificationLevel = guild.VerificationLevel;
            this.ExplicitContentFilter = guild.ExplicitContentFilter;
            this.SplashId = guild.SplashId;
            this.DiscoverySplashId = guild.DiscoverySplashId;
            this.AFKChannelId = guild.AFKChannelId;
            this.WidgetChannelId = guild.WidgetChannelId;
            this.SafetyAlertsChannelId = guild.SafetyAlertsChannelId;
            this.SystemChannelId = guild.SystemChannelId;
            this.RulesChannelId = guild.RulesChannelId;
            this.PublicUpdatesChannelId = guild.PublicUpdatesChannelId;
            this.OwnerId = guild.OwnerId;
            this.ApplicationId = guild.ApplicationId;
            this.PremiumTier = guild.PremiumTier;
            this.BannerId = guild.BannerId;
            this.VanityURLCode = guild.VanityURLCode;
            this.SystemChannelFlags = guild.SystemChannelFlags;
            this.Description = guild.Description;
            this.PremiumSubscriptionCount = guild.PremiumSubscriptionCount;
            this.MaxPresences = guild.MaxPresences;
            this.MaxMembers = guild.MaxMembers;
            this.MaxVideoChannelUsers = guild.MaxVideoChannelUsers;
            this.MaxStageVideoChannelUsers = guild.MaxStageVideoChannelUsers;
            this.PreferredLocale = guild.PreferredLocale;
            this.PreferredCulture = guild.PreferredCulture;
            this.IsBoostProgressBarEnabled = guild.IsBoostProgressBarEnabled;
            this.InventorySettings = guild.InventorySettings;
            this.IncidentsData = guild.IncidentsData;

            IReadOnlyCollection<RestRole> guildRoles = guild.Roles;
            ImmutableDictionary<ulong, FetchedRoleInfo>.Builder roles = ImmutableDictionary.CreateBuilder<ulong, FetchedRoleInfo>();
            foreach (RestRole guildRole in guildRoles)
            {
                if (!this.roles.TryGetValue(guildRole.Id, out FetchedRoleInfo? role))
                {
                    role = new FetchedRoleInfo(this, guildRole.Id);
                }

                role.Update(guildRole);
                roles.Add(guildRole.Id, role);
            }
            this.roles = roles.ToImmutable();
            this.everyoneRole = this.roles[this.Guild.Id];
            this.emotes = guild.Emotes.ToImmutableArray();

            IReadOnlyCollection<CustomSticker> guildStickers = guild.Stickers;

            if (guildStickers.Count > 0)
            {
                ImmutableArray<FetchedCustomStickerInfo>.Builder stickers = ImmutableArray.CreateBuilder<FetchedCustomStickerInfo>(guildStickers.Count);
                foreach (CustomSticker guildSticker in guildStickers)
                {
                    FetchedCustomStickerInfo sticker = this.stickers.FirstOrDefault(x => x.Id == guildSticker.Id) ?? new FetchedCustomStickerInfo(this, guildSticker.Id);

                    sticker.Update(guildSticker);
                    stickers.Add(sticker);
                }
                this.stickers = stickers.ToImmutable();
            }
            else
                this.stickers = ImmutableArray<FetchedCustomStickerInfo>.Empty;
        }

        internal void Update(IGuild guild)
        {
            this.AfkTimeout = guild.AFKTimeout;
            this.IsWidgetEnabled = guild.IsWidgetEnabled;
            this.DefaultMessageNotifications = guild.DefaultMessageNotifications;
            this.MfaLevel = guild.MfaLevel;
            this.VerificationLevel = guild.VerificationLevel;
            this.ExplicitContentFilter = guild.ExplicitContentFilter;
            this.SplashId = guild.SplashId;
            this.DiscoverySplashId = guild.DiscoverySplashId;
            this.AFKChannelId = guild.AFKChannelId;
            this.WidgetChannelId = guild.WidgetChannelId;
            this.SafetyAlertsChannelId = guild.SafetyAlertsChannelId;
            this.SystemChannelId = guild.SystemChannelId;
            this.RulesChannelId = guild.RulesChannelId;
            this.PublicUpdatesChannelId = guild.PublicUpdatesChannelId;
            this.OwnerId = guild.OwnerId;
            this.ApplicationId = guild.ApplicationId;
            this.PremiumTier = guild.PremiumTier;
            this.BannerId = guild.BannerId;
            this.VanityURLCode = guild.VanityURLCode;
            this.SystemChannelFlags = guild.SystemChannelFlags;
            this.Description = guild.Description;
            this.PremiumSubscriptionCount = guild.PremiumSubscriptionCount;
            this.MaxPresences = guild.MaxPresences;
            this.MaxMembers = guild.MaxMembers;
            this.MaxVideoChannelUsers = guild.MaxVideoChannelUsers;
            this.MaxStageVideoChannelUsers = guild.MaxStageVideoChannelUsers;
            this.PreferredLocale = guild.PreferredLocale;
            this.PreferredCulture = guild.PreferredCulture;
            this.IsBoostProgressBarEnabled = guild.IsBoostProgressBarEnabled;
            this.InventorySettings = guild.InventorySettings;
            this.IncidentsData = guild.IncidentsData;

            IReadOnlyCollection<IRole> guildRoles = guild.Roles;
            ImmutableDictionary<ulong, FetchedRoleInfo>.Builder roles = ImmutableDictionary.CreateBuilder<ulong, FetchedRoleInfo>();
            foreach (IRole guildRole in guildRoles)
            {
                if (!this.roles.TryGetValue(guildRole.Id, out FetchedRoleInfo? role))
                {
                    role = new FetchedRoleInfo(this, guildRole.Id);
                }

                role.Update(guildRole);
                roles.Add(guildRole.Id, role);
            }
            this.roles = roles.ToImmutable();
            this.everyoneRole = this.roles[this.Guild.Id];
            this.emotes = guild.Emotes.ToImmutableArray();

            IReadOnlyCollection<ICustomSticker> guildStickers = guild.Stickers;

            if (guildStickers.Count > 0)
            {
                ImmutableArray<FetchedCustomStickerInfo>.Builder stickers = ImmutableArray.CreateBuilder<FetchedCustomStickerInfo>(guildStickers.Count);
                foreach (ICustomSticker guildSticker in guildStickers)
                {
                    FetchedCustomStickerInfo sticker = this.stickers.FirstOrDefault(x => x.Id == guildSticker.Id) ?? new FetchedCustomStickerInfo(this, guildSticker.Id);

                    sticker.Update(guildSticker);
                    stickers.Add(sticker);
                }
                this.stickers = stickers.ToImmutable();
            }
            else
                this.stickers = ImmutableArray<FetchedCustomStickerInfo>.Empty;
        }
    }

    private sealed class TempCurrentUserInformation
    {
        public bool IsOwner { get; }
        public string Permissions { get; }
        private GuildPermissions? CachedPermissions { get; set; }

        public TempCurrentUserInformation(UserGuild model)
        {
            this.IsOwner = model.Owner;
            this.Permissions = model.Permissions;
        }

        public UserGuild ToModel(OAuthGuild guild)
        {
            return new UserGuild()
            {
                Id = guild.Id,
                Owner = this.IsOwner,
                Permissions = this.Permissions,
                Icon = guild.IconId,
                ApproximateMemberCount = Optional.CreateFromNullable(guild.ApproximateMemberCount),
                ApproximatePresenceCount = Optional.CreateFromNullable(guild.ApproximatePresenceCount),
                Features = guild.Features,
                Name = guild.Name,
            };
        }

        public GuildPermissions GetPermissions()
        {
            if (this.CachedPermissions.HasValue)
                return this.CachedPermissions.Value;
            GuildPermissions result = new(this.Permissions);
            this.CachedPermissions = result;
            return result;
        }
    }
}
