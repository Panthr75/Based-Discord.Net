using Discord.Audio;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Model = Discord.API.Guild;
using WidgetModel = Discord.API.GuildWidget;

namespace Discord.Rest
{
    /// <summary>
    ///     Represents a REST-based guild/server.
    /// </summary>
    [DebuggerDisplay(@"{DebuggerDisplay,nq}")]
    public class RestGuild : RestEntity<ulong>, IGuild, IUpdateable
    {
        #region RestGuild
        private ConcurrentDictionary<ulong, RestRole> _roles;
        private ConcurrentDictionary<ulong, GuildEmote> _emotes;
        private ConcurrentDictionary<ulong, CustomSticker> _stickers;

        /// <inheritdoc />
        public string Name { get; private set; }
        /// <inheritdoc />
        public int AFKTimeout { get; private set; }
        /// <inheritdoc />
        public bool IsWidgetEnabled { get; private set; }
        /// <inheritdoc />
        public VerificationLevel VerificationLevel { get; private set; }
        /// <inheritdoc />
        public MfaLevel MfaLevel { get; private set; }
        /// <inheritdoc />
        public DefaultMessageNotifications DefaultMessageNotifications { get; private set; }
        /// <inheritdoc />
        public ExplicitContentFilterLevel ExplicitContentFilter { get; private set; }

        /// <inheritdoc />
        public ulong? AFKChannelId { get; private set; }
        /// <inheritdoc />
        public ulong? WidgetChannelId { get; private set; }
        /// <inheritdoc />
        public ulong? SafetyAlertsChannelId { get; private set; }
        /// <inheritdoc />
        public ulong? SystemChannelId { get; private set; }
        /// <inheritdoc />
        public ulong? RulesChannelId { get; private set; }
        /// <inheritdoc />
        public ulong? PublicUpdatesChannelId { get; private set; }
        /// <inheritdoc />
        public ulong OwnerId { get; private set; }
        /// <inheritdoc />
        [Obsolete("Use IAudioChannel.RTCRegion instead")]
        public string? VoiceRegionId { get; private set; }
        /// <inheritdoc />
        public string? IconId { get; private set; }
        /// <inheritdoc />
        public string? SplashId { get; private set; }
        /// <inheritdoc />
        public string? DiscoverySplashId { get; private set; }
        internal bool Available { get; private set; }
        /// <inheritdoc />
        public ulong? ApplicationId { get; private set; }
        /// <inheritdoc />
        public PremiumTier PremiumTier { get; private set; }
        /// <inheritdoc />
        public string? BannerId { get; private set; }
        /// <inheritdoc />
        public string? VanityURLCode { get; private set; }
        /// <inheritdoc />
        public SystemChannelMessageDeny SystemChannelFlags { get; private set; }
        /// <inheritdoc />
        public string? Description { get; private set; }
        /// <inheritdoc />
        public int PremiumSubscriptionCount { get; private set; }
        /// <inheritdoc />
        public string PreferredLocale { get; private set; }
        /// <inheritdoc />
        public int? MaxPresences { get; private set; }
        /// <inheritdoc />
        public int? MaxMembers { get; private set; }
        /// <inheritdoc />
        public int? MaxVideoChannelUsers { get; private set; }
        /// <inheritdoc />
        public int? MaxStageVideoChannelUsers { get; private set; }
        /// <inheritdoc />
        public int? ApproximateMemberCount { get; private set; }
        /// <inheritdoc />
        public int? ApproximatePresenceCount { get; private set; }
        /// <inheritdoc/>
        public int MaxBitrate
        {
            get
            {
                return PremiumTier switch
                {
                    PremiumTier.Tier1 => 128000,
                    PremiumTier.Tier2 => 256000,
                    PremiumTier.Tier3 => 384000,
                    _ => 96000,
                };
            }
        }
        /// <inheritdoc/>
        public ulong MaxUploadLimit
            => GuildHelper.GetUploadLimit(this);
        /// <inheritdoc />
        public NsfwLevel NsfwLevel { get; private set; }
        /// <inheritdoc />
        public bool IsBoostProgressBarEnabled { get; private set; }
        /// <inheritdoc />
        public CultureInfo PreferredCulture { get; private set; }
        /// <inheritdoc />
        public GuildFeatures Features { get; private set; }

        /// <inheritdoc/>
        public GuildIncidentsData IncidentsData { get; private set; }

        /// <inheritdoc />
        public GuildInventorySettings? InventorySettings { get; private set; }

        /// <inheritdoc />
        public DateTimeOffset CreatedAt => SnowflakeUtils.FromSnowflake(Id);

        /// <inheritdoc />
        public string? IconUrl => CDN.GetGuildIconUrl(Id, IconId);
        /// <inheritdoc />
        public string? SplashUrl => CDN.GetGuildSplashUrl(Id, SplashId);
        /// <inheritdoc />
        public string? DiscoverySplashUrl => CDN.GetGuildDiscoverySplashUrl(Id, DiscoverySplashId);
        /// <inheritdoc />
        public string? BannerUrl => CDN.GetGuildBannerUrl(Id, BannerId, ImageFormat.Auto);

        /// <summary>
        ///     Gets the built-in role containing all users in this guild.
        /// </summary>
        public RestRole EveryoneRole => GetRole(Id)!;

        /// <summary>
        ///     Gets a collection of all roles in this guild.
        /// </summary>
        public IReadOnlyCollection<RestRole> Roles => _roles.ToReadOnlyCollection();
        /// <inheritdoc />
        public IReadOnlyCollection<GuildEmote> Emotes => _emotes.ToReadOnlyCollection();

        /// <summary>
        ///     Gets a collection of all custom stickers for this guild.
        /// </summary>
        /// <returns>
        ///     A read-only collection of all custom stickers for this guild.
        /// </returns>
        public IReadOnlyCollection<CustomSticker> Stickers => _stickers.ToReadOnlyCollection();

        internal RestGuild(BaseDiscordClient client, ulong id)
            : base(client, id)
        {
            _roles = new();
            _emotes = new();
            _stickers = new();
            Name = string.Empty;
            PreferredLocale = "en-US";
            PreferredCulture = CultureInfo.GetCultureInfo("en-US");
            Features = new GuildFeatures(GuildFeature.None, null);
            IncidentsData = new();
        }
        internal static RestGuild Create(BaseDiscordClient discord, Model model)
        {
            var entity = new RestGuild(discord, model.Id);
            entity.Update(model);
            return entity;
        }
        internal void Update(Model model)
        {
            AFKChannelId = model.AFKChannelId;
            if (model.WidgetChannelId.IsSpecified)
                WidgetChannelId = model.WidgetChannelId.Value;
            if (model.SafetyAlertsChannelId.IsSpecified)
                SafetyAlertsChannelId = model.SafetyAlertsChannelId.Value;
            SystemChannelId = model.SystemChannelId;
            RulesChannelId = model.RulesChannelId;
            PublicUpdatesChannelId = model.PublicUpdatesChannelId;
            AFKTimeout = model.AFKTimeout;
            if (model.WidgetEnabled.IsSpecified)
                IsWidgetEnabled = model.WidgetEnabled.Value;
            IconId = model.Icon;
            Name = model.Name;
            OwnerId = model.OwnerId;
#pragma warning disable CS0618 // Type or member is obsolete [Set obsolete to obsolete]
            VoiceRegionId = model.Region;
#pragma warning restore CS0618 // Type or member is obsolete
            SplashId = model.Splash;
            DiscoverySplashId = model.DiscoverySplash;
            VerificationLevel = model.VerificationLevel;
            MfaLevel = model.MfaLevel;
            DefaultMessageNotifications = model.DefaultMessageNotifications;
            ExplicitContentFilter = model.ExplicitContentFilter;
            ApplicationId = model.ApplicationId;
            PremiumTier = model.PremiumTier;
            VanityURLCode = model.VanityURLCode;
            BannerId = model.Banner;
            SystemChannelFlags = model.SystemChannelFlags;
            Description = model.Description;
            PremiumSubscriptionCount = model.PremiumSubscriptionCount.GetValueOrDefault();
            NsfwLevel = model.NsfwLevel;
            IncidentsData = model.IncidentsData is not null
                ? new GuildIncidentsData { DmsDisabledUntil = model.IncidentsData.DmsDisabledUntil, InvitesDisabledUntil = model.IncidentsData.InvitesDisabledUntil }
                : new GuildIncidentsData();
            if (model.MaxPresences.IsSpecified)
                MaxPresences = model.MaxPresences.Value ?? 25000;
            if (model.MaxMembers.IsSpecified)
                MaxMembers = model.MaxMembers.Value;
            if (model.MaxVideoChannelUsers.IsSpecified)
                MaxVideoChannelUsers = model.MaxVideoChannelUsers.Value;
            if (model.MaxStageVideoChannelUsers.IsSpecified)
                MaxStageVideoChannelUsers = model.MaxStageVideoChannelUsers.Value;
            PreferredLocale = model.PreferredLocale;
            PreferredCulture = new CultureInfo(PreferredLocale);
            if (model.ApproximateMemberCount.IsSpecified)
                ApproximateMemberCount = model.ApproximateMemberCount.Value;
            if (model.ApproximatePresenceCount.IsSpecified)
                ApproximatePresenceCount = model.ApproximatePresenceCount.Value;
            if (model.IsBoostProgressBarEnabled.IsSpecified)
                IsBoostProgressBarEnabled = model.IsBoostProgressBarEnabled.Value;
            if (model.InventorySettings.IsSpecified)
                InventorySettings = model.InventorySettings.Value is null ? null : new(model.InventorySettings.Value.IsEmojiPackCollectible.GetValueOrDefault(false));

            if (model.Emojis != null)
            {
                var emotes = new ConcurrentDictionary<ulong, GuildEmote>(ConcurrentHashSet.DefaultConcurrencyLevel, (int)(model.Emojis.Length * 1.05));

                for (int i = 0; i < model.Emojis.Length; i++)
                {
                    var entity = model.Emojis[i].ToEntity();
                    emotes.TryAdd(entity.Id, entity);
                }
                _emotes = emotes;
            }

            Features = model.Features;

            if (model.Roles != null)
            {
                var roles = new ConcurrentDictionary<ulong, RestRole>(ConcurrentHashSet.DefaultConcurrencyLevel, (int)(model.Roles.Length * 1.05));

                for (int i = 0; i < model.Roles.Length; i++)
                {
                    var role = model.Roles[i];

                    if (!_roles.TryGetValue(role.Id, out var entity))
                    {
                        entity = RestRole.Create(Discord, this, role);
                    }
                    else
                    {
                        entity.Update(role);
                    }

                    roles.TryAdd(role.Id, entity);
                }

                _roles = roles;
            }

            if (model.Stickers != null)
            {
                var stickers = new ConcurrentDictionary<ulong, CustomSticker>(ConcurrentHashSet.DefaultConcurrencyLevel, (int)(model.Stickers.Length * 1.05));

                for (int i = 0; i < model.Stickers.Length; i++)
                {
                    var sticker = model.Stickers[i];

                    if (_stickers.TryGetValue(sticker.Id, out var entity))
                    {
                        entity.Update(sticker);
                    }
                    else
                    {
                        entity = CustomSticker.Create(Discord, sticker, this, sticker.User.IsSpecified ? sticker.User.Value.Id : null);
                    }

                    stickers.TryAdd(entity.Id, entity);
                }

                _stickers = stickers;
            }

            Available = true;
        }
        internal void Update(WidgetModel model)
        {
            WidgetChannelId = model.ChannelId;
            IsWidgetEnabled = model.Enabled;
        }
        #endregion

        #region General
        /// <inheritdoc />
        public async Task UpdateAsync(RequestOptions? options = null)
        {
            var model = await Discord.ApiClient.GetGuildAsync(Id, false, options).ConfigureAwait(false);
            if (model is not null)
            {
                Update(model);
            }
        }
        /// <summary>
        ///     Updates this object's properties with its current state.
        /// </summary>
        /// <param name="withCounts">
        ///     If true, <see cref="ApproximateMemberCount"/> and <see cref="ApproximatePresenceCount"/>
        ///     will be updated as well.
        /// </param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <remarks>
        ///     If <paramref name="withCounts"/> is true, <see cref="ApproximateMemberCount"/> and
        ///     <see cref="ApproximatePresenceCount"/> will be updated as well.
        /// </remarks>
        public async Task UpdateAsync(bool withCounts, RequestOptions? options = null)
        {
            var model = await Discord.ApiClient.GetGuildAsync(Id, withCounts, options).ConfigureAwait(false);
            if (model is not null)
            {
                Update(model);
            }
        }

        /// <inheritdoc />
        public Task DeleteAsync(RequestOptions? options = null)
            => GuildHelper.DeleteAsync(this, Discord, options);

        /// <inheritdoc />
        public async Task ModifyAsync(Action<GuildProperties> func, RequestOptions? options = null)
        {
            var model = await GuildHelper.ModifyAsync(this, Discord, func, options).ConfigureAwait(false);
            Update(model);
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        public async Task ModifyWidgetAsync(Action<GuildWidgetProperties> func, RequestOptions? options = null)
        {
            var model = await GuildHelper.ModifyWidgetAsync(this, Discord, func, options).ConfigureAwait(false);
            Update(model);
        }

        /// <inheritdoc />
        public Task LeaveAsync(RequestOptions? options = null)
            => GuildHelper.LeaveAsync(this, Discord, options);

        /// <inheritdoc />
        public async Task<GuildIncidentsData> ModifyIncidentActionsAsync(Action<GuildIncidentsDataProperties> props, RequestOptions? options = null)
        {
            IncidentsData = await GuildHelper.ModifyGuildIncidentActionsAsync(this, Discord, props, options);

            return IncidentsData;
        }
        #endregion

        #region Interactions
        /// <summary>
        ///     Deletes all slash commands in the current guild.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous delete operation.
        /// </returns>
        public Task DeleteSlashCommandsAsync(RequestOptions? options = null)
            => InteractionHelper.DeleteAllGuildCommandsAsync(Discord, Id, options);

        /// <summary>
        ///     Gets a collection of slash commands created by the current user in this guild.
        /// </summary>
        /// <param name="withLocalizations">Whether to include full localization dictionaries in the returned objects, instead of the name localized and description localized fields.</param>
        /// <param name="locale">The target locale of the localized name and description fields. Sets <c>X-Discord-Locale</c> header, which takes precedence over <c>Accept-Language</c>.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a read-only collection of
        ///     slash commands created by the current user.
        /// </returns>
        public Task<IReadOnlyCollection<RestGuildCommand>> GetSlashCommandsAsync(bool withLocalizations = false, string? locale = null, RequestOptions? options = null)
            => GuildHelper.GetSlashCommandsAsync(this, Discord, withLocalizations, locale, options);

        /// <summary>
        ///     Gets a slash command in the current guild.
        /// </summary>
        /// <param name="id">The unique identifier of the slash command.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a
        ///     slash command created by the current user.
        /// </returns>
        public Task<RestGuildCommand?> GetSlashCommandAsync(ulong id, RequestOptions? options = null)
            => GuildHelper.GetSlashCommandAsync(this, id, Discord, options);
        #endregion

        #region Bans

        /// <inheritdoc cref="IGuild.GetBansAsync(int, RequestOptions)" />
        public IAsyncEnumerable<IReadOnlyCollection<RestBan>> GetBansAsync(int limit = DiscordConfig.MaxBansPerBatch, RequestOptions? options = null)
            => GuildHelper.GetBansAsync(this, Discord, null, Direction.Before, limit, options);

        /// <inheritdoc cref="IGuild.GetBansAsync(ulong, Direction, int, RequestOptions)" />
        public IAsyncEnumerable<IReadOnlyCollection<RestBan>> GetBansAsync(ulong fromUserId, Direction dir, int limit = DiscordConfig.MaxBansPerBatch, RequestOptions? options = null)
            => GuildHelper.GetBansAsync(this, Discord, fromUserId, dir, limit, options);

        /// <inheritdoc cref="IGuild.GetBansAsync(IUser, Direction, int, RequestOptions)" />
        public IAsyncEnumerable<IReadOnlyCollection<RestBan>> GetBansAsync(IUser fromUser, Direction dir, int limit = DiscordConfig.MaxBansPerBatch, RequestOptions? options = null)
            => GuildHelper.GetBansAsync(this, Discord, fromUser.Id, dir, limit, options);
        /// <summary>
        ///     Gets a ban object for a banned user.
        /// </summary>
        /// <param name="user">The banned user.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a ban object, which
        ///     contains the user information and the reason for the ban; <see langword="null"/> if the ban entry cannot be found.
        /// </returns>
        public Task<RestBan?> GetBanAsync(IUser user, RequestOptions? options = null)
            => GuildHelper.GetBanAsync(this, Discord, user.Id, options);
        /// <summary>
        ///     Gets a ban object for a banned user.
        /// </summary>
        /// <param name="userId">The snowflake identifier for the banned user.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a ban object, which
        ///     contains the user information and the reason for the ban; <see langword="null"/> if the ban entry cannot be found.
        /// </returns>
        public Task<RestBan?> GetBanAsync(ulong userId, RequestOptions? options = null)
            => GuildHelper.GetBanAsync(this, Discord, userId, options);

        /// <inheritdoc />
        public Task AddBanAsync(IUser user, int pruneDays = 0, string? reason = null, RequestOptions? options = null)
            => GuildHelper.AddBanAsync(this, Discord, user.Id, pruneDays, reason, options);
        /// <inheritdoc />
        public Task AddBanAsync(ulong userId, int pruneDays = 0, string? reason = null, RequestOptions? options = null)
            => GuildHelper.AddBanAsync(this, Discord, userId, pruneDays, reason, options);

        /// <inheritdoc />
        public Task RemoveBanAsync(IUser user, RequestOptions? options = null)
            => GuildHelper.RemoveBanAsync(this, Discord, user.Id, options);
        /// <inheritdoc />
        public Task RemoveBanAsync(ulong userId, RequestOptions? options = null)
            => GuildHelper.RemoveBanAsync(this, Discord, userId, options);
        #endregion

        #region Channels
        /// <summary>
        ///     Gets a collection of all channels in this guild.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a read-only collection of
        ///     generic channels found within this guild.
        /// </returns>
        public Task<IReadOnlyCollection<RestGuildChannel>> GetChannelsAsync(RequestOptions? options = null)
            => GuildHelper.GetChannelsAsync(this, Discord, options);

        /// <summary>
        ///     Gets a channel in this guild.
        /// </summary>
        /// <param name="id">The snowflake identifier for the channel.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains the generic channel
        ///     associated with the specified <paramref name="id"/>; <see langword="null"/> if none is found.
        /// </returns>
        public Task<RestGuildChannel?> GetChannelAsync(ulong id, RequestOptions? options = null)
            => GuildHelper.GetChannelAsync(this, Discord, id, options);

        /// <inheritdoc />
        public Task ReorderChannelsAsync(IEnumerable<ReorderChannelProperties> args, RequestOptions? options = null)
        {
            var arr = args.ToArray();
            return GuildHelper.ReorderChannelsAsync(this, Discord, arr, options);
        }

        /// <summary>
        ///     Deletes a channel from this guild.
        /// </summary>
        /// <remarks>
        ///     <note type="warning">
        ///         No checks are made to ensure the channel exists before it's deleted. This can result in
        ///         Not Found <see cref="Discord.Net.HttpException">Http Exceptions</see>.
        ///     </note>
        ///     <note>
        ///         The bot needs the <see cref="ChannelPermission.ManageChannels">MANAGE_CHANNELS</see>
        ///         permission inside the channel in order to delete it.
        ///     </note>
        /// </remarks>
        /// <param name="id">The snowflake identifier for the channel.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous deletion operation.
        /// </returns>
        public Task DeleteChannelAsync(ulong id, RequestOptions? options = null)
            => ChannelHelper.DeleteChannelAsync(id, Discord, options);

        #region Text Channels
        /// <summary>
        ///     Gets a collection of all text channels in this guild.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a read-only collection of
        ///     message channels found within this guild.
        /// </returns>
        public Task<IReadOnlyCollection<RestTextChannel>> GetTextChannelsAsync(RequestOptions? options = null)
            => GuildHelper.GetTextChannelsAsync(this, Discord, options);

        /// <summary>
        ///     Gets a text channel in this guild.
        /// </summary>
        /// <param name="id">The snowflake identifier for the text channel.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains the text channel
        ///     associated with the specified <paramref name="id"/>; <see langword="null"/> if none is found.
        /// </returns>
        public async Task<RestTextChannel?> GetTextChannelAsync(ulong id, RequestOptions? options = null)
        {
            var channel = await GuildHelper.GetChannelAsync(this, Discord, id, options).ConfigureAwait(false);
            return channel as RestTextChannel;
        }

        /// <summary>
        ///     Creates a new text channel in this guild.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageChannels">MANAGE_CHANNELS</see>
        ///         permission inside the guild in order to create text channels.
        ///     </note>
        /// </remarks>
        /// <example>
        ///     The following example creates a new text channel under an existing category named <c>Wumpus</c> with a set topic.
        ///     <code language="cs">
        ///     var categories = await guild.GetCategoriesAsync();
        ///     var targetCategory = categories.FirstOrDefault(x => x.Name == "wumpus");
        ///     if (targetCategory == null) return;
        ///     await Context.Guild.CreateTextChannelAsync(name, x =>
        ///     {
        ///         x.CategoryId = targetCategory.Id;
        ///         x.Topic = $"This channel was created at {DateTimeOffset.UtcNow} by {user}.";
        ///     });
        ///     </code>
        /// </example>
        /// <param name="name">The new name for the text channel.</param>
        /// <param name="func">The delegate containing the properties to be applied to the channel upon its creation.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous creation operation. The task result contains the newly created
        ///     text channel.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />.</exception>
        public Task<RestTextChannel> CreateTextChannelAsync(string name, Action<TextChannelProperties>? func = null, RequestOptions? options = null)
            => GuildHelper.CreateTextChannelAsync(this, Discord, name, options, func);

        /// <summary>
        ///     Modifies a text channel in this guild.
        /// </summary>
        /// <remarks>
        ///     <note type="warning">
        ///         No checks are made to ensure the channel exists before it's modified. This can result in
        ///         Not Found <see cref="Discord.Net.HttpException">Http Exceptions</see>.
        ///     </note>
        ///     <note>
        ///         The bot needs the <see cref="ChannelPermission.ManageChannels">MANAGE_CHANNELS</see>
        ///         permission inside the channel in order to modify it.
        ///     </note>
        /// </remarks>
        /// <param name="id">The snowflake identifier for the text channel.</param>
        /// <param name="func">The delegate containing the properties to modify the channel with.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous modification operation. The task result contains the modified text channel.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        public async Task<RestTextChannel> ModifyTextChannelAsync(ulong id, Action<TextChannelProperties> func, RequestOptions? options = null)
        {
            var model = await ChannelHelper.ModifyTextChannelAsync(id, Discord, func, options).ConfigureAwait(false);
            return (RestTextChannel)RestGuildChannel.Create(Discord, this, model);
        }
        #endregion

        #region News Channels
        /// <summary>
        ///     Gets a collection of all announcement channels in this guild.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         Announcement channels are only available in Community guilds.
        ///     </note>
        /// </remarks>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a read-only collection of
        ///     announcement channels found within this guild.
        /// </returns>
        public Task<IReadOnlyCollection<RestNewsChannel>> GetNewsChannelsAsync(RequestOptions? options = null)
            => GuildHelper.GetNewsChannelsAsync(this, Discord, options);

        /// <summary>
        ///     Gets an announcement channel in this guild.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         Announcement channels are only available in Community guilds.
        ///     </note>
        /// </remarks>
        /// <param name="id">The snowflake identifier for the announcement channel.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains the announcement channel
        ///     associated with the specified <paramref name="id"/>; <see langword="null" /> if none is found.
        /// </returns>
        public async Task<RestNewsChannel?> GetNewsChannelAsync(ulong id, RequestOptions? options = null)
        {
            var channel = await GuildHelper.GetChannelAsync(this, Discord, id, options).ConfigureAwait(false);
            return channel as RestNewsChannel;
        }

        /// <summary>
        ///     Creates a new announcement channel in this guild.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageChannels">MANAGE_CHANNELS</see>
        ///         permission inside the guild in order to create announcement channels.
        ///     </note>
        ///     <note>
        ///         Announcement channels are only available in Community guilds.
        ///     </note>
        /// </remarks>
        /// <param name="name">The new name for the announcement channel.</param>
        /// <param name="func">The delegate containing the properties to be applied to the channel upon its creation.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous creation operation. The task result contains the newly created
        ///     announcement channel.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />.</exception>
        public Task<RestNewsChannel> CreateNewsChannelAsync(string name, Action<TextChannelProperties>? func = null, RequestOptions? options = null)
            => GuildHelper.CreateNewsChannelAsync(this, Discord, name, options, func);

        /// <summary>
        ///     Modifies an announcement channel in this guild.
        /// </summary>
        /// <remarks>
        ///     <note type="warning">
        ///         No checks are made to ensure the channel exists before it's modified. This can result in
        ///         Not Found <see cref="Discord.Net.HttpException">Http Exceptions</see>.
        ///     </note>
        ///     <note>
        ///         The bot needs the <see cref="ChannelPermission.ManageChannels">MANAGE_CHANNELS</see>
        ///         permission inside the channel in order to modify it.
        ///     </note>
        ///     <note>
        ///         Announcement channels are only available in Community guilds.
        ///     </note>
        /// </remarks>
        /// <param name="id">The snowflake identifier for the announcement channel.</param>
        /// <param name="func">The delegate containing the properties to modify the channel with.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous modification operation. The task result contains the modified announcement channel.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        public async Task<RestNewsChannel> ModifyNewsChannelAsync(ulong id, Action<TextChannelProperties> func, RequestOptions? options = null)
        {
            var model = await ChannelHelper.ModifyNewsChannelAsync(id, Discord, func, options).ConfigureAwait(false);
            return (RestNewsChannel)RestGuildChannel.Create(Discord, this, model);
        }
        #endregion

        #region Thread Channels
        /// <summary>
        ///     Gets a collection of all thread in this guild.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a read-only collection of
        ///     threads found within this guild.
        /// </returns>
        public Task<IReadOnlyCollection<RestThreadChannel>> GetThreadChannelsAsync(RequestOptions? options = null)
            => GuildHelper.GetThreadChannelsAsync(this, Discord, options);

        /// <summary>
        ///     Gets a thread channel in this guild.
        /// </summary>
        /// <param name="id">The snowflake identifier for the thread channel.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains the thread channel associated
        ///     with the specified <paramref name="id"/>; <see langword="null"/> if none is found.
        /// </returns>
        public async Task<RestThreadChannel?> GetThreadChannelAsync(ulong id, RequestOptions? options = null)
        {
            var channel = await GuildHelper.GetChannelAsync(this, Discord, id, options).ConfigureAwait(false);
            return channel as RestThreadChannel;
        }
        #endregion

        #region Voice Channels
        /// <summary>
        ///     Gets a collection of all voice channels in this guild.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a read-only collection of
        ///     voice channels found within this guild.
        /// </returns>
        public Task<IReadOnlyCollection<RestVoiceChannel>> GetVoiceChannelsAsync(RequestOptions? options = null)
            => GuildHelper.GetVoiceChannelsAsync(this, Discord, options);

        /// <summary>
        ///     Gets a voice channel in this guild.
        /// </summary>
        /// <param name="id">The snowflake identifier for the voice channel.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains the voice channel associated
        ///     with the specified <paramref name="id"/>; <see langword="null" /> if none is found.
        /// </returns>
        public async Task<RestVoiceChannel?> GetVoiceChannelAsync(ulong id, RequestOptions? options = null)
        {
            var channel = await GuildHelper.GetChannelAsync(this, Discord, id, options).ConfigureAwait(false);
            return channel as RestVoiceChannel;
        }

        /// <summary>
        ///     Creates a new voice channel in this guild.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageChannels">MANAGE_CHANNELS</see>
        ///         permission inside the guild in order to create voice channels.
        ///     </note>
        /// </remarks>
        /// <param name="name">The new name for the voice channel.</param>
        /// <param name="func">The delegate containing the properties to be applied to the channel upon its creation.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous creation operation. The task result contains the newly created
        ///     voice channel.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />.</exception>
        public Task<RestVoiceChannel> CreateVoiceChannelAsync(string name, Action<VoiceChannelProperties>? func = null, RequestOptions? options = null)
            => GuildHelper.CreateVoiceChannelAsync(this, Discord, name, options, func);

        /// <summary>
        ///     Modifies a voice channel in this guild.
        /// </summary>
        /// <remarks>
        ///     <note type="warning">
        ///         No checks are made to ensure the channel exists before it's modified. This can result in
        ///         Not Found <see cref="Discord.Net.HttpException">Http Exceptions</see>.
        ///     </note>
        ///     <note>
        ///         The bot needs the <see cref="ChannelPermission.ManageChannels">MANAGE_CHANNELS</see>
        ///         permission inside the channel in order to modify it.
        ///     </note>
        /// </remarks>
        /// <param name="id">The snowflake identifier for the voice channel.</param>
        /// <param name="func">The delegate containing the properties to modify the channel with.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous modification operation. The task result contains the modified voice channel.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        public async Task<RestVoiceChannel> ModifyVoiceChannelAsync(ulong id, Action<VoiceChannelProperties> func, RequestOptions? options = null)
        {
            var model = await ChannelHelper.ModifyVoiceChannelAsync(id, Discord, func, options).ConfigureAwait(false);
            return (RestVoiceChannel)RestGuildChannel.Create(Discord, this, model);
        }
        #endregion

        #region Stage Channels
        /// <summary>
        ///     Gets a collection of all stage channels in this guild.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a read-only collection of
        ///     stage channels found within this guild.
        /// </returns>
        public Task<IReadOnlyCollection<RestStageChannel>> GetStageChannelsAsync(RequestOptions? options = null)
            => GuildHelper.GetStageChannelsAsync(this, Discord, options);

        /// <summary>
        ///     Gets a stage channel in this guild.
        /// </summary>
        /// <param name="id">The snowflake identifier for the stage channel.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains the stage channel associated
        ///     with the specified <paramref name="id"/>; <see langword="null" /> if none is found.
        /// </returns>
        public async Task<RestStageChannel?> GetStageChannelAsync(ulong id, RequestOptions? options = null)
        {
            var channel = await GuildHelper.GetChannelAsync(this, Discord, id, options).ConfigureAwait(false);
            return channel as RestStageChannel;
        }

        /// <summary>
        ///     Creates a new stage channel in this guild.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageChannels">MANAGE_CHANNELS</see>
        ///         permission inside the guild in order to create stage channels.
        ///     </note>
        /// </remarks>
        /// <param name="name">The new name for the stage channel.</param>
        /// <param name="func">The delegate containing the properties to be applied to the channel upon its creation.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous creation operation. The task result contains the newly created
        ///     stage channel.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />.</exception>
        public Task<RestStageChannel> CreateStageChannelAsync(string name, Action<VoiceChannelProperties>? func = null, RequestOptions? options = null)
            => GuildHelper.CreateStageChannelAsync(this, Discord, name, options, func);

        /// <summary>
        ///     Modifies a stage channel in this guild.
        /// </summary>
        /// <remarks>
        ///     <note type="warning">
        ///         No checks are made to ensure the channel exists before it's modified. This can result in
        ///         Not Found <see cref="Discord.Net.HttpException">Http Exceptions</see>.
        ///     </note>
        ///     <note>
        ///         The bot needs the <see cref="ChannelPermission.ManageChannels">MANAGE_CHANNELS</see>
        ///         permission inside the channel in order to modify it.
        ///     </note>
        /// </remarks>
        /// <param name="id">The snowflake identifier for the stage channel.</param>
        /// <param name="func">The delegate containing the properties to modify the channel with.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous modification operation. The task result contains the modified stage channel.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        public async Task<RestStageChannel> ModifyStageChannelAsync(ulong id, Action<VoiceChannelProperties> func, RequestOptions? options = null)
        {
            var model = await ChannelHelper.ModifyStageChannelAsync(id, Discord, func, options).ConfigureAwait(false);
            return (RestStageChannel)RestGuildChannel.Create(Discord, this, model);
        }
        #endregion

        #region Forum Channels
        /// <summary>
        ///     Gets a collection of all forum channels in this guild.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a read-only collection of
        ///     forum channels found within this guild.
        /// </returns>
        public Task<IReadOnlyCollection<RestForumChannel>> GetForumChannelsAsync(RequestOptions? options = null)
            => GuildHelper.GetForumChannelsAsync(this, Discord, options);

        /// <summary>
        ///     Gets a forum channel in this guild.
        /// </summary>
        /// <param name="id">The snowflake identifier for the forum channel.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains the forum channel associated
        ///     with the specified <paramref name="id"/>; <see langword="null" /> if none is found.
        /// </returns>
        public async Task<RestForumChannel?> GetForumChannelAsync(ulong id, RequestOptions? options = null)
        {
            var channel = await GuildHelper.GetChannelAsync(this, Discord, id, options).ConfigureAwait(false);
            return channel as RestForumChannel;
        }

        /// <summary>
        ///     Creates a new channel forum in this guild.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageChannels">MANAGE_CHANNELS</see>
        ///         permission inside the guild in order to create forum channels.
        ///     </note>
        /// </remarks>
        /// <param name="name">The new name for the forum.</param>
        /// <param name="func">The delegate containing the properties to be applied to the channel upon its creation.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous creation operation. The task result contains the newly created
        ///     forum channel.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />.</exception>
        public Task<RestForumChannel> CreateForumChannelAsync(string name, Action<ForumChannelProperties>? func = null, RequestOptions? options = null)
            => GuildHelper.CreateForumChannelAsync(this, Discord, name, options, func);

        /// <summary>
        ///     Modifies a forum channel in this guild.
        /// </summary>
        /// <remarks>
        ///     <note type="warning">
        ///         No checks are made to ensure the channel exists before it's modified. This can result in
        ///         Not Found <see cref="Discord.Net.HttpException">Http Exceptions</see>.
        ///     </note>
        ///     <note>
        ///         The bot needs the <see cref="ChannelPermission.ManageChannels">MANAGE_CHANNELS</see>
        ///         permission inside the channel in order to modify it.
        ///     </note>
        /// </remarks>
        /// <param name="id">The snowflake identifier for the forum channel.</param>
        /// <param name="func">The delegate containing the properties to modify the channel with.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous modification operation. The task result contains the modified forum channel.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        public async Task<RestForumChannel> ModifyForumChannelAsync(ulong id, Action<ForumChannelProperties> func, RequestOptions? options = null)
        {
            var model = await ChannelHelper.ModifyForumChannelAsync(id, Discord, func, options).ConfigureAwait(false);
            return (RestForumChannel)RestGuildChannel.Create(Discord, this, model);
        }
        #endregion

        #region Media Channels
        /// <summary>
        ///     Gets a collection of all media channels in this guild.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a read-only collection of
        ///     media channels found within this guild.
        /// </returns>
        public Task<IReadOnlyCollection<RestMediaChannel>> GetMediaChannelsAsync(RequestOptions? options = null)
            => GuildHelper.GetMediaChannelsAsync(this, Discord, options);

        /// <summary>
        ///     Gets a media channel in this guild.
        /// </summary>
        /// <param name="id">The snowflake identifier for the media channel.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains the media channel associated
        ///     with the specified <paramref name="id"/>; <see langword="null" /> if none is found.
        /// </returns>
        public async Task<RestMediaChannel?> GetMediaChannelAsync(ulong id, RequestOptions? options = null)
        {
            var channel = await GuildHelper.GetChannelAsync(this, Discord, id, options).ConfigureAwait(false);
            return channel as RestMediaChannel;
        }

        /// <summary>
        ///     Creates a new media channel in this guild.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageChannels">MANAGE_CHANNELS</see>
        ///         permission inside the guild in order to create media channels.
        ///     </note>
        /// </remarks>
        /// <param name="name">The new name for the media channel.</param>
        /// <param name="func">The delegate containing the properties to be applied to the channel upon its creation.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous creation operation. The task result contains the newly created
        ///     media channel.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />.</exception>
        public Task<RestMediaChannel> CreateMediaChannelAsync(string name, Action<ForumChannelProperties>? func = null, RequestOptions? options = null)
            => GuildHelper.CreateMediaChannelAsync(this, Discord, name, options, func);

        /// <summary>
        ///     Modifies a media channel in this guild.
        /// </summary>
        /// <remarks>
        ///     <note type="warning">
        ///         No checks are made to ensure the channel exists before it's modified. This can result in
        ///         Not Found <see cref="Discord.Net.HttpException">Http Exceptions</see>.
        ///     </note>
        ///     <note>
        ///         The bot needs the <see cref="ChannelPermission.ManageChannels">MANAGE_CHANNELS</see>
        ///         permission inside the channel in order to modify it.
        ///     </note>
        /// </remarks>
        /// <param name="id">The snowflake identifier for the media channel.</param>
        /// <param name="func">The delegate containing the properties to modify the channel with.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous modification operation. The task result contains the modified media channel.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        public async Task<RestMediaChannel> ModifyMediaChannelAsync(ulong id, Action<ForumChannelProperties> func, RequestOptions? options = null)
        {
            var model = await ChannelHelper.ModifyMediaChannelAsync(id, Discord, func, options).ConfigureAwait(false);
            return (RestMediaChannel)RestGuildChannel.Create(Discord, this, model);
        }
        #endregion

        #region Category Channels
        /// <summary>
        ///     Gets a collection of all category channels in this guild.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a read-only collection of
        ///     category channels found within this guild.
        /// </returns>
        public async Task<IReadOnlyCollection<RestCategoryChannel>> GetCategoryChannelsAsync(RequestOptions? options = null)
        {
            var channels = await GuildHelper.GetChannelsAsync(this, Discord, options).ConfigureAwait(false);
            return channels.OfType<RestCategoryChannel>().ToImmutableArray();
        }

        /// <summary>
        ///     Gets a category channel in this guild.
        /// </summary>
        /// <param name="id">The snowflake identifier for the category channel.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains the category channel associated
        ///     with the specified <paramref name="id"/>; <see langword="null" /> if none is found.
        /// </returns>
        public async Task<RestCategoryChannel?> GetCategoryChannelAsync(ulong id, RequestOptions? options = null)
        {
            var channel = await GuildHelper.GetChannelAsync(this, Discord, id, options).ConfigureAwait(false);
            return channel as RestCategoryChannel;
        }

        /// <summary>
        ///     Creates a new channel category in this guild.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageChannels">MANAGE_CHANNELS</see>
        ///         permission inside the guild in order to create category channels.
        ///     </note>
        /// </remarks>
        /// <param name="name">The new name for the category.</param>
        /// <param name="func">The delegate containing the properties to be applied to the channel upon its creation.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous creation operation. The task result contains the newly created
        ///     category channel.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />.</exception>
        public Task<RestCategoryChannel> CreateCategoryChannelAsync(string name, Action<GuildChannelProperties>? func = null, RequestOptions? options = null)
            => GuildHelper.CreateCategoryChannelAsync(this, Discord, name, options, func);

        /// <summary>
        ///     Modifies a category channel in this guild.
        /// </summary>
        /// <remarks>
        ///     <note type="warning">
        ///         No checks are made to ensure the channel exists before it's modified. This can result in
        ///         Not Found <see cref="Discord.Net.HttpException">Http Exceptions</see>.
        ///     </note>
        ///     <note>
        ///         The bot needs the <see cref="ChannelPermission.ManageChannels">MANAGE_CHANNELS</see>
        ///         permission inside the channel in order to modify it.
        ///     </note>
        /// </remarks>
        /// <param name="id">The snowflake identifier for the category channel.</param>
        /// <param name="func">The delegate containing the properties to modify the channel with.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous modification operation. The task result contains the modified category channel.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        public async Task<RestCategoryChannel> ModifyCategoryChannelAsync(ulong id, Action<GuildChannelProperties> func, RequestOptions? options = null)
        {
            var model = await ChannelHelper.ModifyCategoryChannelAsync(id, Discord, func, options).ConfigureAwait(false);
            return (RestCategoryChannel)RestGuildChannel.Create(Discord, this, model);
        }
        #endregion

        #region Special Channels
        /// <summary>
        ///     Gets the AFK voice channel in this guild.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains the voice channel that the
        ///     AFK users will be moved to after they have idled for too long; <see langword="null"/> if none is set.
        /// </returns>
        public async Task<RestVoiceChannel?> GetAFKChannelAsync(RequestOptions? options = null)
        {
            var afkId = AFKChannelId;
            if (afkId.HasValue)
            {
                var channel = await GuildHelper.GetChannelAsync(this, Discord, afkId.Value, options).ConfigureAwait(false);
                return channel as RestVoiceChannel;
            }
            return null;
        }

        /// <summary>
        ///     Gets the first viewable text channel in this guild.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains the first viewable text
        ///     channel in this guild; <see langword="null"/> if none is found.
        /// </returns>
        public async Task<RestTextChannel?> GetDefaultChannelAsync(RequestOptions? options = null)
        {
            var channels = await GetTextChannelsAsync(options).ConfigureAwait(false);
            var user = await GetCurrentUserAsync(options).ConfigureAwait(false);
            if (user == null)
            {
                return null;
            }
            return channels
                .Where(c => user.GetPermissions(c).ViewChannel)
                .OrderBy(c => c.Position)
                .FirstOrDefault();
        }

        /// <summary>
        ///     Gets the widget channel (i.e. the channel set in the guild's widget settings) in this guild.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains the widget channel set
        ///     within the server's widget settings; <see langword="null"/> if none is set.
        /// </returns>
        public Task<RestGuildChannel?> GetWidgetChannelAsync(RequestOptions? options = null)
        {
            var widgetChannelId = WidgetChannelId;
            if (widgetChannelId.HasValue)
                return GuildHelper.GetChannelAsync(this, Discord, widgetChannelId.Value, options);
            return Task.FromResult<RestGuildChannel?>(null);
        }

        /// <summary>
        ///     Gets the text channel where guild notices such as welcome messages and boost events are posted.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains the text channel
        ///     where guild notices such as welcome messages and boost events are post; <see langword="null"/> if none is found.
        /// </returns>
        public async Task<RestTextChannel?> GetSystemChannelAsync(RequestOptions? options = null)
        {
            var systemId = SystemChannelId;
            if (systemId.HasValue)
            {
                var channel = await GuildHelper.GetChannelAsync(this, Discord, systemId.Value, options).ConfigureAwait(false);
                return channel as RestTextChannel;
            }
            return null;
        }

        /// <summary>
        ///     Gets the text channel where Community guilds can display rules and/or guidelines.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains the text channel
        ///     where Community guilds can display rules and/or guidelines; <see langword="null"/> if none is set.
        /// </returns>
        public async Task<RestTextChannel?> GetRulesChannelAsync(RequestOptions? options = null)
        {
            var rulesChannelId = RulesChannelId;
            if (rulesChannelId.HasValue)
            {
                var channel = await GuildHelper.GetChannelAsync(this, Discord, rulesChannelId.Value, options).ConfigureAwait(false);
                return channel as RestTextChannel;
            }
            return null;
        }

        /// <summary>
        ///     Gets the text channel where admins and moderators of Community guilds receive notices from Discord.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains the text channel where
        ///     admins and moderators of Community guilds receive notices from Discord; <see langword="null"/> if none is set.
        /// </returns>
        public async Task<RestTextChannel?> GetPublicUpdatesChannelAsync(RequestOptions? options = null)
        {
            var publicUpdatesChannelId = PublicUpdatesChannelId;
            if (publicUpdatesChannelId.HasValue)
            {
                var channel = await GuildHelper.GetChannelAsync(this, Discord, publicUpdatesChannelId.Value, options).ConfigureAwait(false);
                return channel as RestTextChannel;
            }
            return null;
        }
        #endregion

        #endregion

        #region Integrations
        /// <summary>
        ///     Gets a collection of all the integrations this guild contains.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageGuild">MANAGE_GUILD</see>
        ///         permission inside the guild in order to get it's integrations.
        ///     </note>
        /// </remarks>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a read-only collection of
        ///     integrations the guild can has.
        /// </returns>
        public Task<IReadOnlyCollection<RestIntegration>> GetIntegrationsAsync(RequestOptions? options = null)
            => GuildHelper.GetIntegrationsAsync(this, Discord, options);

        /// <summary>
        ///     Deletes an integration.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageGuild">MANAGE_GUILD</see>
        ///         permission inside the guild in order to delete integrations.
        ///     </note>
        /// </remarks>
        /// <param name="id">The id for the integration.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous removal operation.
        /// </returns>
        public Task DeleteIntegrationAsync(ulong id, RequestOptions? options = null)
            => GuildHelper.DeleteIntegrationAsync(this, Discord, id, options);
        #endregion

        #region Invites
        /// <summary>
        ///     Gets a collection of all invites in this guild.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageGuild">MANAGE_GUILD</see>
        ///         permission inside the guild in order to get it's invites.
        ///     </note>
        /// </remarks>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a read-only collection of
        ///     invite metadata, each representing information for an invite found within this guild.
        /// </returns>
        public Task<IReadOnlyCollection<RestInviteMetadata>> GetInvitesAsync(RequestOptions? options = null)
            => GuildHelper.GetInvitesAsync(this, Discord, options);

        /// <summary>
        ///     Gets the vanity invite URL of this guild.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageGuild">MANAGE_GUILD</see>
        ///         permission inside the guild in order to get it's vanity invite.
        ///     </note>
        /// </remarks>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains the partial metadata of
        ///     the vanity invite found within this guild; <see langword="null" /> if none is found.
        /// </returns>
        public Task<RestInviteMetadata?> GetVanityInviteAsync(RequestOptions? options = null)
            => GuildHelper.GetVanityInviteAsync(this, Discord, options);
        #endregion

        #region Roles
        /// <summary>
        ///     Gets a role in this guild.
        /// </summary>
        /// <param name="id">The snowflake identifier for the role.</param>
        /// <returns>
        ///     A role that is associated with the specified <paramref name="id"/>; <see langword="null"/> if none is found.
        /// </returns>
        public RestRole? GetRole(ulong id)
        {
            if (_roles.TryGetValue(id, out RestRole? value))
                return value;
            return null;
        }

        /// <summary>
        ///     Creates a new role with the provided name.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageRoles">MANAGE_ROLES</see>
        ///         permission inside the guild in order to create new roles.
        ///     </note>
        /// </remarks>
        /// <param name="name">The new name for the role.</param>
        /// <param name="permissions">The guild permission that the role should possess.</param>
        /// <param name="color">The color of the role.</param>
        /// <param name="isHoisted">Whether the role is separated from others on the sidebar.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <param name="isMentionable">Whether the role can be mentioned.</param>
        /// <param name="icon">The icon for the role.</param>
        /// <param name="emoji">The unicode emoji to be used as an icon for the role.</param>
        /// <returns>
        ///     A task that represents the asynchronous creation operation. The task result contains the newly created
        ///     role.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />.</exception>
        public async Task<RestRole> CreateRoleAsync(string name, GuildPermissions? permissions = default(GuildPermissions?), Color? color = default(Color?),
            bool isHoisted = false, bool isMentionable = false, RequestOptions? options = null, Image? icon = null, Emoji? emoji = null)
        {
            var model = await GuildHelper.CreateRoleAsync(this, Discord, name, permissions, color, isHoisted, isMentionable, options, icon, emoji).ConfigureAwait(false);

            if (_roles.TryGetValue(model.Id, out var entity))
            {
                entity.Update(model);
                return entity;
            }

            entity = RestRole.Create(Discord, this, model);
            _roles[entity.Id] = entity;

            return entity;
        }

        /// <inheritdoc />
        public async Task ReorderRolesAsync(IEnumerable<ReorderRoleProperties> args, RequestOptions? options = null)
        {
            var models = await GuildHelper.ReorderRolesAsync(this, Discord, args, options).ConfigureAwait(false);
            foreach (var model in models)
            {
                if (_roles.TryGetValue(model.Id, out var entity))
                {
                    entity.Update(model);
                    continue;
                }

                _roles[model.Id] = RestRole.Create(Discord, this, model);
            }
        }

        /// <summary>
        ///     Modifies a role in this guild.
        /// </summary>
        /// <remarks>
        ///     <note type="warning">
        ///         No checks are made to ensure the role exists before it's modified. This can result in
        ///         Not Found <see cref="Discord.Net.HttpException">Http Exceptions</see>.
        ///     </note>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageRoles">MANAGE_ROLES</see>
        ///         permission inside the guild in order to modify roles.
        ///     </note>
        /// </remarks>
        /// <param name="id">The snowflake identifier for the role.</param>
        /// <param name="func">A delegate containing the properties to modify the role with.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous modification operation. The task result contains the modified
        ///     role.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="func" /> is <see langword="null"/>.</exception>
        public async Task<RestRole> ModifyRoleAsync(ulong id, Action<RoleProperties> func, RequestOptions? options = null)
        {
            var model = await RoleHelper.ModifyAsync(this, id, Discord, func, options).ConfigureAwait(false);
            if (_roles.TryGetValue(model.Id, out var role))
            {
                role.Update(model);
                return role;
            }

            role = RestRole.Create(Discord, this, model);
            _roles[model.Id] = role;
            return role;
        }

        /// <inheritdoc />
        public async Task DeleteRoleAsync(ulong id, RequestOptions? options = null)
        {
            await RoleHelper.DeleteAsync(this, id, Discord, options).ConfigureAwait(false);
            _roles.TryRemove(id, out _);
        }

        #endregion

        #region Users
        /// <summary>
        ///     Gets a collection of all users in this guild.
        /// </summary>
        /// <remarks>
        ///     This method retrieves all users found within this guild.
        ///     <note type="important">
        ///         Getting users in a guild requires the <see cref="GatewayIntents.GuildMembers">GUILD_MEMBERS </see> Privileged Intent to be enabled for your application.
        ///     </note>
        /// </remarks>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a collection of guild
        ///     users found within this guild.
        /// </returns>
        public IAsyncEnumerable<IReadOnlyCollection<RestGuildUser>> GetUsersAsync(RequestOptions? options = null)
            => GuildHelper.GetUsersAsync(this, Discord, null, null, options);

        /// <summary>
        ///     Adds a user to this guild.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         This method requires you have an OAuth2 access token for the user, requested with the <c>guilds.join</c> scope, and that the bot has the <see cref="GuildPermission.CreateInstantInvite"/> permission in the guild.
        ///     </note>
        /// </remarks>
        /// <param name="userId">The snowflake identifier of the user.</param>
        /// <param name="accessToken">The OAuth2 access token for the user, requested with the guilds.join scope.</param>
        /// <param name="func">The delegate containing the properties to be applied to the user upon being added to the guild.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>A guild user associated with the specified <paramref name="userId" />; <see langword="null" /> if the user is already in the guild.</returns>
        public Task<RestGuildUser?> AddGuildUserAsync(ulong userId, string accessToken, Action<AddGuildUserProperties>? func = null, RequestOptions? options = null)
            => GuildHelper.AddGuildUserAsync(this, Discord, userId, accessToken, func, options);

        /// <summary>
        ///     Gets a user from this guild.
        /// </summary>
        /// <remarks>
        ///     This method retrieves a user found within this guild.
        /// </remarks>
        /// <param name="id">The snowflake identifier of the user.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains the guild user
        ///     associated with the specified <paramref name="id"/>; <see langword="null"/> if none is found.
        /// </returns>
        public Task<RestGuildUser?> GetUserAsync(ulong id, RequestOptions? options = null)
            => GuildHelper.GetUserAsync(this, Discord, id, options);

        /// <summary>
        ///     Gets the current user for this guild.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains the currently logged-in
        ///     user within this guild.
        /// </returns>
        public Task<RestGuildUser?> GetCurrentUserAsync(RequestOptions? options = null)
            => GuildHelper.GetUserAsync(this, Discord, Discord.CurrentUser.Id, options);

        /// <summary>
        ///     Gets the owner of this guild.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains the owner of this guild.
        /// </returns>
        public Task<RestGuildUser?> GetOwnerAsync(RequestOptions? options = null)
            => GuildHelper.GetUserAsync(this, Discord, OwnerId, options);

        /// <inheritdoc />
        public Task<int> PruneUsersAsync(int days = 30, bool simulate = false, RequestOptions? options = null, IEnumerable<ulong>? includeRoleIds = null)
            => GuildHelper.PruneUsersAsync(this, Discord, days, simulate, options, includeRoleIds);

        /// <summary>
        ///     Gets a collection of users in this guild that the name or nickname starts with the
        ///     provided <see cref="string"/> at <paramref name="query"/>.
        /// </summary>
        /// <remarks>
        ///     The <paramref name="limit"/> can not be higher than <see cref="DiscordConfig.MaxUsersPerBatch"/>.
        /// </remarks>
        /// <param name="query">The partial name or nickname to search.</param>
        /// <param name="limit">The maximum number of users to be gotten.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a collection of guild
        ///     users that the name or nickname starts with the provided <see cref="string"/> at <paramref name="query"/>.
        /// </returns>
        public Task<IReadOnlyCollection<RestGuildUser>> SearchUsersAsync(string query, int limit = DiscordConfig.MaxUsersPerBatch, RequestOptions? options = null)
            => GuildHelper.SearchUsersAsync(this, Discord, query ?? string.Empty, limit, options);
        #endregion

        #region Audit logs
        /// <summary>
        ///     Gets the specified number of audit log entries for this guild.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ViewAuditLog">VIEW_AUDIT_LOG</see>
        ///         permission inside the guild in order to view it's audit log entries.
        ///     </note>
        /// </remarks>
        /// <param name="limit">The number of audit log entries to fetch.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <param name="beforeId">The audit log entry ID to get entries before.</param>
        /// <param name="actionType">The type of actions to filter.</param>
        /// <param name="userId">The user ID to filter entries for.</param>
        /// <param name="afterId">The audit log entry ID to get entries after.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a read-only collection
        ///     of the requested audit log entries.
        /// </returns>
        public IAsyncEnumerable<IReadOnlyCollection<RestAuditLogEntry>> GetAuditLogsAsync(int limit, RequestOptions? options = null, ulong? beforeId = null, ulong? userId = null, ActionType? actionType = null, ulong? afterId = null)
            => GuildHelper.GetAuditLogsAsync(this, Discord, beforeId, limit, options, userId: userId, actionType: actionType, afterId: afterId);
        #endregion

        #region Voice
        /// <summary>
        /// Disconnects the user from its current voice channel
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.MoveMembers">MOVE_MEMBERS</see>
        ///         permission inside the guild to disconnect <paramref name="user"/>.
        ///     </note>
        /// </remarks>
        /// <param name="user">The user to disconnect.</param>
        /// <returns>A task that represents the asynchronous operation for disconnecting a user.</returns>
        public Task DisconnectAsync(IGuildUser user)
            => user.ModifyAsync(x => x.Channel = null);

        /// <summary>
        /// Moves the user to the voice channel.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.MoveMembers">MOVE_MEMBERS</see>
        ///         permission inside the guild AND must have the <see cref="ChannelPermission.Connect">CONNECT</see>
        ///         permission inside <paramref name="targetChannel"/> in order to move
        ///         <paramref name="user"/> to it.
        ///     </note>
        /// </remarks>
        /// <param name="user">The user to move.</param>
        /// <param name="targetChannel">the channel where the user gets moved to.</param>
        /// <returns>A task that represents the asynchronous operation for moving a user.</returns>
        public Task MoveAsync(IGuildUser user, IVoiceChannel targetChannel)
            => user.ModifyAsync(x => x.Channel = new Optional<IVoiceChannel?>(targetChannel));

        /// <summary>
        ///     Gets a collection of all the voice regions this guild can access.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a read-only collection of
        ///     voice regions the guild can access.
        /// </returns>
        public Task<IReadOnlyCollection<RestVoiceRegion>> GetVoiceRegionsAsync(RequestOptions? options = null)
            => GuildHelper.GetVoiceRegionsAsync(this, Discord, options);
        #endregion

        #region Webhooks
        /// <summary>
        ///     Gets a collection of all webhook from this guild.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageWebhooks">MANAGE_WEBHOOKS</see>
        ///         permission inside the guild in order to view the it's webhooks.
        ///     </note>
        /// </remarks>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a read-only collection
        ///     of webhooks found within the guild.
        /// </returns>
        public Task<IReadOnlyCollection<RestWebhook>> GetWebhooksAsync(RequestOptions? options = null)
            => GuildHelper.GetWebhooksAsync(this, Discord, options);

        /// <summary>
        ///     Gets a webhook found within this guild.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageWebhooks">MANAGE_WEBHOOKS</see>
        ///         permission inside the guild in order to view the it's webhooks.
        ///     </note>
        /// </remarks>
        /// <param name="id">The identifier for the webhook.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains the webhook with the
        ///     specified <paramref name="id"/>; <see langword="null"/> if none is found.
        /// </returns>
        public Task<RestWebhook?> GetWebhookAsync(ulong id, RequestOptions? options = null)
            => GuildHelper.GetWebhookAsync(this, Discord, id, options);
        #endregion

        #region Interactions
        /// <summary>
        ///     Gets the bot's application commands in this guild.
        /// </summary>
        /// <param name="withLocalizations">Whether to include full localization dictionaries in the returned objects, instead of the name localized and description localized fields.</param>
        /// <param name="locale">The target locale of the localized name and description fields. Sets <c>X-Discord-Locale</c> header, which takes precedence over <c>Accept-Language</c>.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a read-only collection
        ///     of application commands created by the bot that were found within the guild.
        /// </returns>
        public Task<IReadOnlyCollection<RestGuildCommand>> GetApplicationCommandsAsync(bool withLocalizations = false, string? locale = null, RequestOptions? options = null)
            => ClientHelper.GetGuildApplicationCommandsAsync(Discord, Id, withLocalizations, locale, options);

        /// <summary>
        ///     Gets the bot's application command within this guild with the specified id.
        /// </summary>
        /// <param name="id">The id of the application command to get.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A Task that represents the asynchronous get operation. The task result contains a <see cref="IApplicationCommand"/>
        ///     if found, otherwise <see langword="null"/>.
        /// </returns>
        public Task<RestGuildCommand?> GetApplicationCommandAsync(ulong id, RequestOptions? options = null)
            => ClientHelper.GetGuildApplicationCommandAsync(Discord, id, Id, options);

        /// <summary>
        ///     Creates an application command within this guild.
        /// </summary>
        /// <param name="properties">The properties to use when creating the command.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous creation operation. The task result contains the command that was created.
        /// </returns>
        public async Task<RestGuildCommand> CreateApplicationCommandAsync(ApplicationCommandProperties properties, RequestOptions? options = null)
        {
            var model = await InteractionHelper.CreateGuildCommandAsync(Discord, Id, properties, options);

            return RestGuildCommand.Create(Discord, model, Id);
        }

        /// <summary>
        ///     Overwrites the bot's application commands within this guild.
        /// </summary>
        /// <param name="properties">A collection of properties to use when creating the commands.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous creation operation. The task result contains a collection of commands that was created.
        /// </returns>
        public async Task<IReadOnlyCollection<RestGuildCommand>> BulkOverwriteApplicationCommandsAsync(ApplicationCommandProperties[] properties,
            RequestOptions? options = null)
        {
            var models = await InteractionHelper.BulkOverwriteGuildCommandsAsync(Discord, Id, properties, options);

            return models.Select(x => RestGuildCommand.Create(Discord, x, Id)).ToImmutableArray();
        }

        /// <summary>
        ///     Modifies the specified application command in this guild.
        /// </summary>
        /// <remarks>
        ///     <note type="warning">
        ///         No checks are made to ensure the application command exists before it's modified. This can result in
        ///         Not Found <see cref="Discord.Net.HttpException">Http Exceptions</see>.
        ///     </note>
        /// </remarks>
        /// <param name="id">The snowflake identifier for the application command.</param>
        /// <param name="func">The new properties to use when modifying the command.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous modification operation. The task result contains the modified application command.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when you pass in an invalid <see cref="ApplicationCommandProperties"/> type.</exception>
        public async Task<RestGuildCommand> ModifyApplicationCommandAsync<TArg>(ulong id, Action<TArg> func, RequestOptions? options)
            where TArg : ApplicationCommandProperties
        {
            var model = await InteractionHelper.ModifyGuildCommandAsync(Discord, id, Id, func, options).ConfigureAwait(false);

            return RestGuildCommand.Create(Discord, model, Id);
        }

        /// <summary>
        ///     Deletes an application command from this guild.
        /// </summary>
        /// <remarks>
        ///     <note type="warning">
        ///         No checks are made to ensure the application command exists before it's deleted. This can result in
        ///         Not Found <see cref="Discord.Net.HttpException">Http Exceptions</see>.
        ///     </note>
        /// </remarks>
        /// <param name="id">The snowflake identifier for the application command.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous deletion operation.
        /// </returns>
        public Task DeleteApplicationCommandAsync(ulong id, RequestOptions? options = null)
            => InteractionHelper.DeleteGuildCommandAsync(Discord, Id, id, options);
        #endregion

        #region Emotes
        /// <inheritdoc />
        public async Task<IReadOnlyCollection<GuildEmote>> GetEmotesAsync(RequestOptions? options = null)
        {
            var entities = await GuildHelper.GetEmotesAsync(this, Discord, options).ConfigureAwait(false);
            var emotes = new ConcurrentDictionary<ulong, GuildEmote>(ConcurrentHashSet.DefaultConcurrencyLevel, (int)(entities.Count * 1.05));
            foreach (var entity in entities)
            {
                emotes[entity.Id] = entity;
            }
            _emotes = emotes;

            return Emotes;
        }
        /// <inheritdoc />
        public GuildEmote? GetEmote(ulong id)
        {
            if (_emotes.TryGetValue(id, out var emote))
                return emote;
            else
                return null;
        }
        /// <inheritdoc />
        public async Task<GuildEmote?> GetEmoteAsync(ulong id, RequestOptions? options = null)
        {
            var emote = await GuildHelper.GetEmoteAsync(this, Discord, id, options).ConfigureAwait(false);
            if (emote is null)
            {
                _emotes.TryRemove(id, out _);
                return null;
            }

            _emotes[id] = emote;
            return emote;
        }
        /// <inheritdoc />
        public async Task<GuildEmote> CreateEmoteAsync(string name, Image image, Optional<IEnumerable<IRole>?> roles = default(Optional<IEnumerable<IRole>?>), RequestOptions? options = null)
        {
            var emote = await GuildHelper.CreateEmoteAsync(this, Discord, name, image, roles, options).ConfigureAwait(false);

            _emotes[emote.Id] = emote;
            return emote;
        }

        /// <inheritdoc />
        public async Task<GuildEmote> ModifyEmoteAsync(GuildEmote emote, Action<EmoteProperties> func, RequestOptions? options = null)
        {
            Preconditions.NotNull(emote, nameof(emote));

            var modifiedEmote = await GuildHelper.ModifyEmoteAsync(this, Discord, emote.Id, func, options).ConfigureAwait(false);

            _emotes[modifiedEmote.Id] = modifiedEmote;
            return modifiedEmote;
        }
        /// <inheritdoc />
        public async Task<GuildEmote> ModifyEmoteAsync(ulong id, Action<EmoteProperties> func, RequestOptions? options = null)
        {
            var emote = await GuildHelper.ModifyEmoteAsync(this, Discord, id, func, options).ConfigureAwait(false);

            _emotes[emote.Id] = emote;
            return emote;
        }
        /// <inheritdoc />
        public async Task DeleteEmoteAsync(ulong id, RequestOptions? options = null)
        {
            await GuildHelper.DeleteEmoteAsync(this, Discord, id, options).ConfigureAwait(false);

            _emotes.TryRemove(id, out _);
        }
        /// <inheritdoc />
        public async Task DeleteEmoteAsync(GuildEmote emote, RequestOptions? options = null)
        {
            Preconditions.NotNull(emote, nameof(emote));

            await GuildHelper.DeleteEmoteAsync(this, Discord, emote.Id, options).ConfigureAwait(false);

            _emotes.TryRemove(emote.Id, out _);
        }
        #endregion

        #region Stickers
        /// <summary>
        ///     Gets a collection of all stickers within this guild.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains a read-only collection
        ///     of stickers found within the guild.
        /// </returns>
        public async Task<IReadOnlyCollection<CustomSticker>> GetStickersAsync(RequestOptions? options = null)
        {
            var models = await Discord.ApiClient.ListGuildStickersAsync(Id, options).ConfigureAwait(false);
            var stickers = new ConcurrentDictionary<ulong, CustomSticker>(ConcurrentHashSet.DefaultConcurrencyLevel, (int)(models.Length * 1.05));
            foreach (var model in models)
            {
                if (_stickers.TryGetValue(model.Id, out var entity))
                {
                    entity.Update(model);
                }
                else
                {
                    entity = CustomSticker.Create(Discord, model, this, model.User.IsSpecified ? model.User.Value.Id : null);
                }

                stickers.TryAdd(model.Id, entity);
            }
            _stickers = stickers;

            return Stickers;
        }

        /// <summary>
        ///     Gets a specific sticker within this guild.
        /// </summary>
        /// <param name="id">The id of the sticker to get.</param>
        /// <returns>
        ///     The sticker found with the specified <paramref name="id"/>; <see langword="null" /> if none
        ///     is found.
        /// </returns>
        public CustomSticker? GetSticker(ulong id)
        {
            if (_stickers.TryGetValue(id, out var sticker))
                return sticker;
            return null;
        }

        /// <summary>
        ///     Gets a specific sticker within this guild.
        /// </summary>
        /// <param name="id">The id of the sticker to get.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation. The task result contains the sticker found with the
        ///     specified <paramref name="id"/>; <see langword="null" /> if none is found.
        /// </returns>
        public async Task<CustomSticker?> GetStickerAsync(ulong id, RequestOptions? options = null)
        {
            var model = await Discord.ApiClient.GetGuildStickerAsync(Id, id, options).ConfigureAwait(false);

            if (model == null)
            {
                _stickers.TryRemove(id, out _);
                return null;
            }

            if (_stickers.TryGetValue(id, out var sticker))
            {
                sticker.Update(model);
                return sticker;
            }

            sticker = CustomSticker.Create(Discord, model, this, model.User.IsSpecified ? model.User.Value.Id : null);
            _stickers[sticker.Id] = sticker;
            return sticker;
        }

        /// <summary>
        ///     Creates a new sticker in this guild.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.CreateGuildExpressions">CREATE_GUILD_EXPRESSIONS</see>
        ///         permission inside the guild in order to create stickers.
        ///     </note>
        /// </remarks>
        /// <param name="name">The name of the sticker.</param>
        /// <param name="description">The description of the sticker.</param>
        /// <param name="tags">The tags of the sticker.</param>
        /// <param name="image">The image of the new emote.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous creation operation. The task result contains the created sticker.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or <paramref name="tags"/> is <see langword="null"/>.</exception>
        public async Task<CustomSticker> CreateStickerAsync(string name, Image image, IEnumerable<string> tags, string? description = null, RequestOptions? options = null)
        {
            var model = await GuildHelper.CreateStickerAsync(Discord, this, name, image, tags, description, options).ConfigureAwait(false);

            if (_stickers.TryGetValue(model.Id, out var sticker))
            {
                sticker.Update(model);
                return sticker;
            }

            sticker = CustomSticker.Create(Discord, model, this, model.User.IsSpecified ? model.User.Value.Id : null);
            _stickers[sticker.Id] = sticker;
            return sticker;
        }
        /// <summary>
        ///     Creates a new sticker in this guild
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.CreateGuildExpressions">CREATE_GUILD_EXPRESSIONS</see>
        ///         permission inside the guild in order to create stickers.
        ///     </note>
        /// </remarks>
        /// <param name="name">The name of the sticker.</param>
        /// <param name="description">The description of the sticker.</param>
        /// <param name="tags">The tags of the sticker.</param>
        /// <param name="path">The path of the file to upload.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous creation operation. The task result contains the created sticker.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/>, <paramref name="path"/>, or <paramref name="tags"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="path"/> is invalidly formatted</exception>
        /// <exception cref="PathTooLongException"><paramref name="path"/> exceeds the system-defined maximum length</exception>
        /// <exception cref="DirectoryNotFoundException"><paramref name="path"/> is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="UnauthorizedAccessException"><paramref name="path"/> is a directory or The caller does not have the required permission.</exception>
        /// <exception cref="FileNotFoundException"><paramref name="path"/> isn't a path to a file.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <exception cref="IOException">An I/O error occurs whilst reading the file at <paramref name="path"/>.</exception>
        public async Task<CustomSticker> CreateStickerAsync(string name, string path, IEnumerable<string> tags, string? description = null,
            RequestOptions? options = null)
        {
            using var fs = File.OpenRead(path);
            return await CreateStickerAsync(name, fs, Path.GetFileName(fs.Name), tags, description, options).ConfigureAwait(false);
        }

        /// <summary>
        ///     Creates a new sticker in this guild
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.CreateGuildExpressions">CREATE_GUILD_EXPRESSIONS</see>
        ///         permission inside the guild in order to create stickers.
        ///     </note>
        /// </remarks>
        /// <param name="name">The name of the sticker.</param>
        /// <param name="description">The description of the sticker.</param>
        /// <param name="tags">The tags of the sticker.</param>
        /// <param name="stream">The stream containing the file data.</param>
        /// <param name="filename">The name of the file <b>with</b> the extension, ex: image.png.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous creation operation. The task result contains the created sticker.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/>, <paramref name="stream"/>, <paramref name="filename"/>, or <paramref name="tags"/> is <see langword="null"/>.</exception>
        public async Task<CustomSticker> CreateStickerAsync(string name, Stream stream, string filename, IEnumerable<string> tags,
             string? description = null, RequestOptions? options = null)
        {
            var model = await GuildHelper.CreateStickerAsync(Discord, this, name, stream, filename, tags, description, options).ConfigureAwait(false);

            if (_stickers.TryGetValue(model.Id, out var sticker))
            {
                sticker.Update(model);
                return sticker;
            }

            sticker = CustomSticker.Create(Discord, model, this, model.User.IsSpecified ? model.User.Value.Id : null);
            _stickers[sticker.Id] = sticker;
            return sticker;
        }
        /// <summary>
        ///     Modifies a sticker in this guild.
        /// </summary>
        /// <remarks>
        ///     <note type="warning">
        ///         No checks are made to ensure the sticker exists before it's modified. This can result in
        ///         Not Found <see cref="Discord.Net.HttpException">Http Exceptions</see>.
        ///     </note>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageEmojisAndStickers">MANAGE_GUILD_EXPRESSIONS</see>
        ///         permission inside the guild in order to modify stickers, unless they were created by the bot,
        ///         in which case only the <see cref="GuildPermission.CreateGuildExpressions">CREATE_GUILD_EXPRESSIONS</see> permission is required.
        ///     </note>
        /// </remarks>
        /// <param name="id">The id of sticker to modify.</param>
        /// <param name="func">A delegate containing the properties to modify the sticker with.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous modification operation. The task result contains the modified sticker.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        public async Task<CustomSticker> ModifyStickerAsync(ulong id, Action<StickerProperties> func, RequestOptions? options = null)
        {
            var model = await GuildHelper.ModifyStickerAsync(Discord, Id, id, func, options).ConfigureAwait(false);

            if (_stickers.TryGetValue(model.Id, out var sticker))
            {
                sticker.Update(model);
                return sticker;
            }

            sticker = CustomSticker.Create(Discord, model, this, model.User.IsSpecified ? model.User.Value.Id : null);
            _stickers[sticker.Id] = sticker;
            return sticker;
        }

        /// <summary>
        ///     Deletes a sticker within this guild.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageEmojisAndStickers">MANAGE_GUILD_EXPRESSIONS</see>
        ///         permission inside the guild in order to delete stickers, unless they were created by the bot,
        ///         in which case only the <see cref="GuildPermission.CreateGuildExpressions">CREATE_GUILD_EXPRESSIONS</see> permission is required.
        ///     </note>
        /// </remarks>
        /// <param name="sticker">The sticker to delete.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous removal operation.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="sticker"/> is <see langword="null"/>.</exception>
        public async Task DeleteStickerAsync(CustomSticker sticker, RequestOptions? options = null)
        {
            Preconditions.NotNull(sticker, nameof(sticker));

            await GuildHelper.DeleteStickerAsync(Discord, Id, sticker.Id, options).ConfigureAwait(false);
            _stickers.TryRemove(sticker.Id, out _);
        }

        /// <summary>
        ///     Deletes a sticker within this guild.
        /// </summary>
        /// <remarks>
        ///     <note type="warning">
        ///         No checks are made to ensure the sticker exists before it's deleted. This can result in
        ///         Not Found <see cref="Discord.Net.HttpException">Http Exceptions</see>.
        ///     </note>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageEmojisAndStickers">MANAGE_GUILD_EXPRESSIONS</see>
        ///         permission inside the guild in order to delete stickers, unless they were created by the bot,
        ///         in which case only the <see cref="GuildPermission.CreateGuildExpressions">CREATE_GUILD_EXPRESSIONS</see> permission is required.
        ///     </note>
        /// </remarks>
        /// <param name="id">The id of sticker to delete.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous removal operation.
        /// </returns>
        public async Task DeleteStickerAsync(ulong id, RequestOptions? options = null)
        {
            await GuildHelper.DeleteStickerAsync(Discord, Id, id, options).ConfigureAwait(false);
            _stickers.TryRemove(id, out _);
        }
        #endregion

        #region Guild Events
        /// <summary>
        ///     Gets all active events within this guild.
        /// </summary>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation.
        /// </returns>
        public Task<IReadOnlyCollection<RestGuildEvent>> GetEventsAsync(RequestOptions? options = null)
            => GuildHelper.GetGuildEventsAsync(Discord, this, options);

        /// <summary>
        ///     Gets an event within this guild.
        /// </summary>
        /// <param name="id">The snowflake identifier for the event.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous get operation.
        /// </returns>
        public Task<RestGuildEvent?> GetEventAsync(ulong id, RequestOptions? options = null)
            => GuildHelper.GetGuildEventAsync(Discord, id, this, options);

        /// <summary>
        ///     Creates an event within this guild.
        /// </summary>
        /// <remarks>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.CreateEvents">CREATE_EVENTS</see> permission and all of the following permisions to create a guild event (based on <paramref name="type"/>):
        ///         <list type="table">
        ///             <item>
        ///                 <term><see cref="GuildScheduledEventType.Stage">STAGE</see></term>
        ///                 <description>
        ///                     The bot in <paramref name="channelId"/> must have the following permissions:
        ///                     <list type="bullet">
        ///                         <item><see cref="ChannelPermission.ManageChannels">MANAGE_CHANNELS</see></item>
        ///                         <item><see cref="ChannelPermission.MuteMembers">MUTE_MEMBERS</see></item>
        ///                         <item><see cref="ChannelPermission.MoveMembers">MOVE_MEMBERS</see></item>
        ///                     </list>
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <term><see cref="GuildScheduledEventType.Voice">VOICE</see></term>
        ///                 <description>
        ///                     The bot in <paramref name="channelId"/> must have the following permissions:
        ///                     <list type="bullet">
        ///                         <item><see cref="ChannelPermission.ViewChannel">VIEW_CHANNEL</see></item>
        ///                         <item><see cref="ChannelPermission.Connect">CONNECT</see></item>
        ///                     </list>
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </note>
        /// </remarks>
        /// <param name="name">The name of the event.</param>
        /// <param name="privacyLevel">The privacy level of the event.</param>
        /// <param name="startTime">The start time of the event.</param>
        /// <param name="type">The type of the event.</param>
        /// <param name="description">The description of the event.</param>
        /// <param name="endTime">The end time of the event.</param>
        /// <param name="channelId">
        ///     The channel id of the event.
        ///     <remarks>
        ///     The event must have a type of <see cref="GuildScheduledEventType.Stage"/> or <see cref="GuildScheduledEventType.Voice"/>
        ///     in order to use this property.
        ///     </remarks>
        /// </param>
        /// <param name="location">The location of the event; links are supported</param>
        /// <param name="coverImage">The optional banner image for the event.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous create operation.
        /// </returns>
        public async Task<RestGuildEvent> CreateEventAsync(
            string name,
            DateTimeOffset startTime,
            GuildScheduledEventType type,
            GuildScheduledEventPrivacyLevel privacyLevel = GuildScheduledEventPrivacyLevel.Private,
            string? description = null,
            DateTimeOffset? endTime = null,
            ulong? channelId = null,
            string? location = null,
            Image? coverImage = null,
            RequestOptions? options = null)
        {
            var model = await GuildHelper.CreateGuildEventAsync(Discord, this, name, privacyLevel, startTime, type, description, endTime, channelId, location, coverImage, options).ConfigureAwait(false);
            return RestGuildEvent.Create(Discord, this, model);
        }


        /// <summary>
        ///     Modifies a guild event in this guild.
        /// </summary>
        /// <remarks>
        ///     <note type="warning">
        ///         No checks are made to ensure the event exists before it's modified. This can result in
        ///         Not Found <see cref="Discord.Net.HttpException">Http Exceptions</see>.
        ///     </note>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageEvents">MANAGE_EVENTS</see>
        ///         or if the event was created by the bot, the <see cref="GuildPermission.CreateGuildExpressions">CREATE_GUILD_EXPRESSIONS</see> permission,
        ///         and all of the following permisions to modify the guild event (based on it's type):
        ///         <list type="table">
        ///             <item>
        ///                 <term><see cref="GuildScheduledEventType.Stage">STAGE</see></term>
        ///                 <description>
        ///                     The bot in the event's channel must have the following permissions:
        ///                     <list type="bullet">
        ///                         <item><see cref="ChannelPermission.ManageChannels">MANAGE_CHANNELS</see></item>
        ///                         <item><see cref="ChannelPermission.MuteMembers">MUTE_MEMBERS</see></item>
        ///                         <item><see cref="ChannelPermission.MoveMembers">MOVE_MEMBERS</see></item>
        ///                     </list>
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <term><see cref="GuildScheduledEventType.Voice">VOICE</see></term>
        ///                 <description>
        ///                     The bot in the event's channel must have the following permissions:
        ///                     <list type="bullet">
        ///                         <item><see cref="ChannelPermission.ViewChannel">VIEW_CHANNEL</see></item>
        ///                         <item><see cref="ChannelPermission.Connect">CONNECT</see></item>
        ///                     </list>
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </note>
        /// </remarks>
        /// <param name="id">The id of the event.</param>
        /// <param name="func">The delegate containing the properties to modify the event with.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous modification operation. The task result contains the modified event.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        public async Task<RestGuildEvent> ModifyEventAsync(ulong id, Action<GuildScheduledEventsProperties> func, RequestOptions? options = null)
        {
            var model = await GuildHelper.ModifyGuildEventAsync(Discord, func, Id, id, options).ConfigureAwait(false);
            return RestGuildEvent.Create(Discord, this, model);
        }

        /// <summary>
        ///     Deletes a guild event from this guild.
        /// </summary>
        /// <remarks>
        ///     <note type="warning">
        ///         No checks are made to ensure the event exists before it's deleted. This can result in
        ///         Not Found <see cref="Discord.Net.HttpException">Http Exceptions</see>.
        ///     </note>
        ///     <note>
        ///         The bot needs the <see cref="GuildPermission.ManageEvents">MANAGE_EVENTS</see>
        ///         or if this event was created by the bot, the <see cref="GuildPermission.CreateGuildExpressions">CREATE_GUILD_EXPRESSIONS</see> permission,
        ///         and all of the following permisions to delete a guild event (based on it's type):
        ///         <list type="table">
        ///             <item>
        ///                 <term><see cref="GuildScheduledEventType.Stage">STAGE</see></term>
        ///                 <description>
        ///                     The bot in the event's channel must have the following permissions:
        ///                     <list type="bullet">
        ///                         <item><see cref="ChannelPermission.ManageChannels">MANAGE_CHANNELS</see></item>
        ///                         <item><see cref="ChannelPermission.MuteMembers">MUTE_MEMBERS</see></item>
        ///                         <item><see cref="ChannelPermission.MoveMembers">MOVE_MEMBERS</see></item>
        ///                     </list>
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <term><see cref="GuildScheduledEventType.Voice">VOICE</see></term>
        ///                 <description>
        ///                     The bot in the event's channel must have the following permissions:
        ///                     <list type="bullet">
        ///                         <item><see cref="ChannelPermission.ViewChannel">VIEW_CHANNEL</see></item>
        ///                         <item><see cref="ChannelPermission.Connect">CONNECT</see></item>
        ///                     </list>
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </note>
        /// </remarks>
        /// <param name="id">The id of the event.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     A task that represents the asynchronous delete operation.
        /// </returns>
        public Task DeleteEventAsync(ulong id, RequestOptions? options = null)
            => GuildHelper.DeleteEventAsync(Discord, Id, id, options);
        #endregion

        #region AutoMod


        /// <inheritdoc cref="IGuild.GetAutoModRuleAsync"/>
        public async Task<RestAutoModRule?> GetAutoModRuleAsync(ulong ruleId, RequestOptions? options = null)
        {
            var rule = await GuildHelper.GetAutoModRuleAsync(ruleId, this, Discord, options);
            if (rule == null)
            {
                return null;
            }

            return RestAutoModRule.Create(Discord, rule);
        }

        /// <inheritdoc cref="IGuild.GetAutoModRulesAsync"/>
        public async Task<RestAutoModRule[]> GetAutoModRulesAsync(RequestOptions? options = null)
        {
            var rules = await GuildHelper.GetAutoModRulesAsync(this, Discord, options);
            return rules.Select(x => RestAutoModRule.Create(Discord, x)).ToArray();
        }

        /// <inheritdoc cref="IGuild.CreateAutoModRuleAsync"/>
        public async Task<RestAutoModRule> CreateAutoModRuleAsync(Action<AutoModRuleProperties> props, RequestOptions? options = null)
        {
            var rule = await GuildHelper.CreateAutoModRuleAsync(this, props, Discord, options);

            return RestAutoModRule.Create(Discord, rule);
        }


        #endregion

        #region Onboarding

        /// <inheritdoc cref="IGuild.GetOnboardingAsync"/>
        public async Task<RestGuildOnboarding> GetOnboardingAsync(RequestOptions? options = null)
        {
            var model = await GuildHelper.GetGuildOnboardingAsync(this, Discord, options);

            return new RestGuildOnboarding(Discord, model, this);
        }

        /// <inheritdoc cref="IGuild.ModifyOnboardingAsync"/>
        public async Task<RestGuildOnboarding> ModifyOnboardingAsync(Action<GuildOnboardingProperties> props, RequestOptions? options = null)
        {
            var model = await GuildHelper.ModifyGuildOnboardingAsync(this, props, Discord, options);

            return new RestGuildOnboarding(Discord, model, this);
        }

        #endregion

        /// <summary>
        ///     Returns the name of the guild.
        /// </summary>
        /// <returns>
        ///     The name of the guild.
        /// </returns>
        public override string ToString() => Name;
        private string DebuggerDisplay => $"{Name} ({Id})";

        #region IGuild
        /// <inheritdoc />
        bool IGuild.Available => Available;
        /// <inheritdoc />
        IAudioClient? IGuild.AudioClient => null;
        /// <inheritdoc />
        IRole IGuild.EveryoneRole => EveryoneRole;
        /// <inheritdoc />
        IReadOnlyCollection<IRole> IGuild.Roles => Roles;
        /// <inheritdoc />
        IReadOnlyCollection<ICustomSticker> IGuild.Stickers => Stickers;
        /// <inheritdoc />
        async Task<IGuildScheduledEvent> IGuild.CreateEventAsync(string name, DateTimeOffset startTime, GuildScheduledEventType type, GuildScheduledEventPrivacyLevel privacyLevel, string? description, DateTimeOffset? endTime, ulong? channelId, string? location, Image? coverImage, RequestOptions? options)
            => await CreateEventAsync(name, startTime, type, privacyLevel, description, endTime, channelId, location, coverImage, options).ConfigureAwait(false);
        /// <inheritdoc />
        async Task<IGuildScheduledEvent?> IGuild.GetEventAsync(ulong id, RequestOptions? options)
            => await GetEventAsync(id, options).ConfigureAwait(false);
        /// <inheritdoc />
        async Task<IReadOnlyCollection<IGuildScheduledEvent>> IGuild.GetEventsAsync(RequestOptions? options)
            => await GetEventsAsync(options).ConfigureAwait(false);
        /// <inheritdoc />
        IAsyncEnumerable<IReadOnlyCollection<IBan>> IGuild.GetBansAsync(int limit, RequestOptions? options)
            => GetBansAsync(limit, options);
        /// <inheritdoc />
        IAsyncEnumerable<IReadOnlyCollection<IBan>> IGuild.GetBansAsync(ulong fromUserId, Direction dir, int limit, RequestOptions? options)
            => GetBansAsync(fromUserId, dir, limit, options);
        /// <inheritdoc />
        IAsyncEnumerable<IReadOnlyCollection<IBan>> IGuild.GetBansAsync(IUser fromUser, Direction dir, int limit, RequestOptions? options)
            => GetBansAsync(fromUser, dir, limit, options);
        /// <inheritdoc/>
        async Task<IBan?> IGuild.GetBanAsync(IUser user, RequestOptions? options)
            => await GetBanAsync(user, options).ConfigureAwait(false);
        /// <inheritdoc/>
        async Task<IBan?> IGuild.GetBanAsync(ulong userId, RequestOptions? options)
            => await GetBanAsync(userId, options).ConfigureAwait(false);

        /// <inheritdoc />
        async Task<IReadOnlyCollection<IGuildChannel>> IGuild.GetChannelsAsync(CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetChannelsAsync(options).ConfigureAwait(false);
            else
                return ImmutableArray<IGuildChannel>.Empty;
        }
        /// <inheritdoc />
        async Task<IGuildChannel?> IGuild.GetChannelAsync(ulong id, CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetChannelAsync(id, options).ConfigureAwait(false);
            else
                return null;
        }
        /// <inheritdoc />
        async Task<IReadOnlyCollection<ITextChannel>> IGuild.GetTextChannelsAsync(CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetTextChannelsAsync(options).ConfigureAwait(false);
            else
                return ImmutableArray<ITextChannel>.Empty;
        }
        /// <inheritdoc />
        async Task<ITextChannel?> IGuild.GetTextChannelAsync(ulong id, CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetTextChannelAsync(id, options).ConfigureAwait(false);
            else
                return null;
        }
        /// <inheritdoc />
        async Task<ITextChannel> IGuild.CreateTextChannelAsync(string name, Action<TextChannelProperties>? func, RequestOptions? options)
            => await CreateTextChannelAsync(name, func, options).ConfigureAwait(false);
        /// <inheritdoc />
        async Task<ITextChannel> IGuild.ModifyTextChannelAsync(ulong id, Action<TextChannelProperties> func, RequestOptions? options)
            => await ModifyTextChannelAsync(id, func, options).ConfigureAwait(false);

        /// <inheritdoc />
        async Task<IReadOnlyCollection<INewsChannel>> IGuild.GetNewsChannelsAsync(CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetNewsChannelsAsync(options).ConfigureAwait(false);
            else
                return ImmutableArray<INewsChannel>.Empty;
        }
        /// <inheritdoc />
        async Task<INewsChannel?> IGuild.GetNewsChannelAsync(ulong id, CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetNewsChannelAsync(id, options).ConfigureAwait(false);
            else
                return null;
        }
        /// <inheritdoc />
        async Task<INewsChannel> IGuild.CreateNewsChannelAsync(string name, Action<TextChannelProperties>? func, RequestOptions? options)
            => await CreateNewsChannelAsync(name, func, options).ConfigureAwait(false);
        /// <inheritdoc />
        async Task<INewsChannel> IGuild.ModifyNewsChannelAsync(ulong id, Action<TextChannelProperties> func, RequestOptions? options)
            => await ModifyNewsChannelAsync(id, func, options).ConfigureAwait(false);

        /// <inheritdoc />
        async Task<IThreadChannel?> IGuild.GetThreadChannelAsync(ulong id, CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetThreadChannelAsync(id, options).ConfigureAwait(false);
            else
                return null;
        }
        /// <inheritdoc />
        async Task<IReadOnlyCollection<IThreadChannel>> IGuild.GetThreadChannelsAsync(CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetThreadChannelsAsync(options).ConfigureAwait(false);
            else
                return ImmutableArray<IThreadChannel>.Empty;
        }

        /// <inheritdoc />
        async Task<IReadOnlyCollection<IVoiceChannel>> IGuild.GetVoiceChannelsAsync(CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetVoiceChannelsAsync(options).ConfigureAwait(false);
            else
                return ImmutableArray<IVoiceChannel>.Empty;
        }
        /// <inheritdoc />
        async Task<IVoiceChannel?> IGuild.GetVoiceChannelAsync(ulong id, CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetVoiceChannelAsync(id, options).ConfigureAwait(false);
            else
                return null;
        }
        /// <inheritdoc />
        async Task<IVoiceChannel> IGuild.CreateVoiceChannelAsync(string name, Action<VoiceChannelProperties>? func, RequestOptions? options)
            => await CreateVoiceChannelAsync(name, func, options).ConfigureAwait(false);
        /// <inheritdoc />
        async Task<IVoiceChannel> IGuild.ModifyVoiceChannelAsync(ulong id, Action<VoiceChannelProperties> func, RequestOptions? options)
            => await ModifyVoiceChannelAsync(id, func, options).ConfigureAwait(false);

        /// <inheritdoc />
        async Task<IReadOnlyCollection<IStageChannel>> IGuild.GetStageChannelsAsync(CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetStageChannelsAsync(options).ConfigureAwait(false);
            else
                return ImmutableArray<IStageChannel>.Empty;
        }
        /// <inheritdoc />
        async Task<IStageChannel?> IGuild.GetStageChannelAsync(ulong id, CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetStageChannelAsync(id, options).ConfigureAwait(false);
            else
                return null;
        }
        /// <inheritdoc />
        async Task<IStageChannel> IGuild.CreateStageChannelAsync(string name, Action<VoiceChannelProperties>? func, RequestOptions? options)
            => await CreateStageChannelAsync(name, func, options).ConfigureAwait(false);
        /// <inheritdoc />
        async Task<IStageChannel> IGuild.ModifyStageChannelAsync(ulong id, Action<VoiceChannelProperties> func, RequestOptions? options)
            => await ModifyStageChannelAsync(id, func, options).ConfigureAwait(false);

        /// <inheritdoc />
        async Task<IForumChannel?> IGuild.GetForumChannelAsync(ulong id, CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetForumChannelAsync(id, options).ConfigureAwait(false);
            else
                return null;
        }
        /// <inheritdoc />
        async Task<IReadOnlyCollection<IForumChannel>> IGuild.GetForumChannelsAsync(CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetForumChannelsAsync(options).ConfigureAwait(false);
            else
                return ImmutableArray<IForumChannel>.Empty;
        }
        /// <inheritdoc />
        async Task<IForumChannel> IGuild.CreateForumChannelAsync(string name, Action<ForumChannelProperties>? func, RequestOptions? options)
            => await CreateForumChannelAsync(name, func, options).ConfigureAwait(false);
        /// <inheritdoc />
        async Task<IForumChannel> IGuild.ModifyForumChannelAsync(ulong id, Action<ForumChannelProperties> func, RequestOptions? options)
            => await ModifyForumChannelAsync(id, func, options).ConfigureAwait(false);

        /// <inheritdoc />
        async Task<IReadOnlyCollection<IMediaChannel>> IGuild.GetMediaChannelsAsync(CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetMediaChannelsAsync(options).ConfigureAwait(false);
            else
                return ImmutableArray<IMediaChannel>.Empty;
        }
        /// <inheritdoc />
        async Task<IMediaChannel?> IGuild.GetMediaChannelAsync(ulong id, CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetMediaChannelAsync(id, options).ConfigureAwait(false);
            else
                return null;
        }
        /// <inheritdoc />
        async Task<IMediaChannel> IGuild.CreateMediaChannelAsync(string name, Action<ForumChannelProperties>? func, RequestOptions? options)
            => await CreateMediaChannelAsync(name, func, options).ConfigureAwait(false);
        /// <inheritdoc />
        async Task<IMediaChannel> IGuild.ModifyMediaChannelAsync(ulong id, Action<ForumChannelProperties> func, RequestOptions? options)
        {
            return await ModifyMediaChannelAsync(id, func, options).ConfigureAwait(false);
        }

        /// <inheritdoc />
        async Task<IReadOnlyCollection<ICategoryChannel>> IGuild.GetCategoryChannelsAsync(CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetCategoryChannelsAsync(options).ConfigureAwait(false);
            else
                return ImmutableArray<ICategoryChannel>.Empty;
        }
        /// <inheritdoc />
        async Task<IReadOnlyCollection<ICategoryChannel>> IGuild.GetCategoriesAsync(CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetCategoryChannelsAsync(options).ConfigureAwait(false);
            else
                return ImmutableArray<ICategoryChannel>.Empty;
        }
        /// <inheritdoc />
        async Task<ICategoryChannel?> IGuild.GetCategoryChannelAsync(ulong id, CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetCategoryChannelAsync(id, options).ConfigureAwait(false);
            else
                return null;
        }
        /// <inheritdoc />
        async Task<ICategoryChannel> IGuild.CreateCategoryChannelAsync(string name, Action<GuildChannelProperties>? func, RequestOptions? options)
            => await CreateCategoryChannelAsync(name, func, options).ConfigureAwait(false);
        /// <inheritdoc />
        async Task<ICategoryChannel> IGuild.CreateCategoryAsync(string name, Action<GuildChannelProperties>? func, RequestOptions? options)
            => await CreateCategoryChannelAsync(name, func, options).ConfigureAwait(false);
        /// <inheritdoc />
        async Task<ICategoryChannel> IGuild.ModifyCategoryChannelAsync(ulong id, Action<GuildChannelProperties> func, RequestOptions? options)
        {
            return await ModifyCategoryChannelAsync(id, func, options).ConfigureAwait(false);
        }

        /// <inheritdoc />
        async Task<IVoiceChannel?> IGuild.GetAFKChannelAsync(CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetAFKChannelAsync(options).ConfigureAwait(false);
            else
                return null;
        }
        /// <inheritdoc />
        async Task<ITextChannel?> IGuild.GetDefaultChannelAsync(CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetDefaultChannelAsync(options).ConfigureAwait(false);
            else
                return null;
        }
        /// <inheritdoc />
        async Task<IGuildChannel?> IGuild.GetWidgetChannelAsync(CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetWidgetChannelAsync(options).ConfigureAwait(false);
            else
                return null;
        }
        /// <inheritdoc />
        async Task<ITextChannel?> IGuild.GetSystemChannelAsync(CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetSystemChannelAsync(options).ConfigureAwait(false);
            else
                return null;
        }
        /// <inheritdoc />
        async Task<ITextChannel?> IGuild.GetRulesChannelAsync(CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetRulesChannelAsync(options).ConfigureAwait(false);
            else
                return null;
        }
        /// <inheritdoc />
        async Task<ITextChannel?> IGuild.GetPublicUpdatesChannelAsync(CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetPublicUpdatesChannelAsync(options).ConfigureAwait(false);
            else
                return null;
        }

        /// <inheritdoc />
        async Task<IReadOnlyCollection<IVoiceRegion>> IGuild.GetVoiceRegionsAsync(RequestOptions? options)
            => await GetVoiceRegionsAsync(options).ConfigureAwait(false);

        /// <inheritdoc />
        async Task<IReadOnlyCollection<IIntegration>> IGuild.GetIntegrationsAsync(RequestOptions? options)
            => await GetIntegrationsAsync(options).ConfigureAwait(false);
        /// <inheritdoc />
        Task IGuild.DeleteIntegrationAsync(ulong id, RequestOptions? options)
            => DeleteIntegrationAsync(id, options);

        /// <inheritdoc />
        async Task<IReadOnlyCollection<IInviteMetadata>> IGuild.GetInvitesAsync(RequestOptions? options)
            => await GetInvitesAsync(options).ConfigureAwait(false);
        /// <inheritdoc />
        async Task<IInviteMetadata?> IGuild.GetVanityInviteAsync(RequestOptions? options)
            => await GetVanityInviteAsync(options).ConfigureAwait(false);

        /// <inheritdoc />
        IRole? IGuild.GetRole(ulong id)
            => GetRole(id);
        /// <inheritdoc />
        async Task<IRole> IGuild.CreateRoleAsync(string name, GuildPermissions? permissions, Color? color, bool isHoisted, RequestOptions? options)
            => await CreateRoleAsync(name, permissions, color, isHoisted, false, options).ConfigureAwait(false);

        /// <inheritdoc />
        async Task<IRole> IGuild.CreateRoleAsync(string name, GuildPermissions? permissions, Color? color, bool isHoisted, bool isMentionable, RequestOptions? options, Image? icon, Emoji? emoji)
            => await CreateRoleAsync(name, permissions, color, isHoisted, isMentionable, options, icon, emoji).ConfigureAwait(false);

        /// <inheritdoc />
        async Task<IRole> IGuild.ModifyRoleAsync(ulong id, Action<RoleProperties> func, RequestOptions? options)
            => await ModifyRoleAsync(id, func, options).ConfigureAwait(false);

        /// <inheritdoc />
        async Task<IGuildUser?> IGuild.AddGuildUserAsync(ulong userId, string accessToken, Action<AddGuildUserProperties>? func, RequestOptions? options)
            => await AddGuildUserAsync(userId, accessToken, func, options).ConfigureAwait(false);

        /// <inheritdoc />
        async Task<IGuildUser?> IGuild.GetUserAsync(ulong id, CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetUserAsync(id, options).ConfigureAwait(false);
            else
                return null;
        }
        /// <inheritdoc />
        async Task<IGuildUser> IGuild.GetCurrentUserAsync(CacheMode mode, RequestOptions? options)
        {
            IGuildUser? user;
            if (mode == CacheMode.AllowDownload)
                user = await GetCurrentUserAsync(options).ConfigureAwait(false);
            else
                user = null;

            if (user == null)
            {
                throw new KeyNotFoundException("Failed to fetch the current user in the guild");
            }
            return user;
        }
        /// <inheritdoc />
        async Task<IGuildUser?> IGuild.GetOwnerAsync(CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await GetOwnerAsync(options).ConfigureAwait(false);
            else
                return null;
        }
        /// <inheritdoc />
        async Task<IReadOnlyCollection<IGuildUser>> IGuild.GetUsersAsync(CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return (await GetUsersAsync(options).FlattenAsync().ConfigureAwait(false)).ToImmutableArray();
            else
                return ImmutableArray<IGuildUser>.Empty;
        }
        /// <inheritdoc />
        /// <exception cref="NotSupportedException">Downloading users is not supported for a REST-based guild.</exception>
        Task IGuild.DownloadUsersAsync() =>
            Task.FromException(new NotSupportedException());
        /// <inheritdoc />
        async Task<IReadOnlyCollection<IGuildUser>> IGuild.SearchUsersAsync(string query, int limit, CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
                return await SearchUsersAsync(query, limit, options).ConfigureAwait(false);
            else
                return ImmutableArray<IGuildUser>.Empty;
        }

        /// <inheritdoc />
        async Task<IReadOnlyCollection<IAuditLogEntry>> IGuild.GetAuditLogsAsync(int limit, CacheMode cacheMode, RequestOptions? options,
            ulong? beforeId, ulong? userId, ActionType? actionType, ulong? afterId)
        {
            if (cacheMode == CacheMode.AllowDownload)
                return (await GetAuditLogsAsync(limit, options, beforeId: beforeId, userId: userId, actionType: actionType, afterId: afterId).FlattenAsync().ConfigureAwait(false)).ToImmutableArray();
            else
                return ImmutableArray<IAuditLogEntry>.Empty;
        }

        /// <inheritdoc />
        async Task<IWebhook?> IGuild.GetWebhookAsync(ulong id, RequestOptions? options)
            => await GetWebhookAsync(id, options).ConfigureAwait(false);
        /// <inheritdoc />
        async Task<IReadOnlyCollection<IWebhook>> IGuild.GetWebhooksAsync(RequestOptions? options)
            => await GetWebhooksAsync(options).ConfigureAwait(false);
        /// <inheritdoc />
        async Task<IReadOnlyCollection<IApplicationCommand>> IGuild.GetApplicationCommandsAsync(bool withLocalizations, string? locale, RequestOptions? options)
            => await GetApplicationCommandsAsync(withLocalizations, locale, options).ConfigureAwait(false);
        /// <inheritdoc />
        async Task<ICustomSticker> IGuild.CreateStickerAsync(string name,  Image image, IEnumerable<string> tags, string? description, RequestOptions? options)
            => await CreateStickerAsync(name, image, tags, description, options).ConfigureAwait(false);
        /// <inheritdoc />
        async Task<ICustomSticker> IGuild.CreateStickerAsync(string name,  Stream stream, string filename, IEnumerable<string> tags, string? description, RequestOptions? options)
            => await CreateStickerAsync(name, stream, filename, tags, description, options).ConfigureAwait(false);
        /// <inheritdoc />
        async Task<ICustomSticker> IGuild.CreateStickerAsync(string name,  string path, IEnumerable<string> tags, string? description, RequestOptions? options)
            => await CreateStickerAsync(name, path, tags, description, options).ConfigureAwait(false);
        /// <inheritdoc />
        ICustomSticker? IGuild.GetSticker(ulong id)
            => GetSticker(id);
        /// <inheritdoc />
        async Task<ICustomSticker?> IGuild.GetStickerAsync(ulong id, CacheMode mode, RequestOptions? options)
        {
            if (mode != CacheMode.AllowDownload)
                return GetSticker(id);

            return await GetStickerAsync(id, options).ConfigureAwait(false);
        }
        /// <inheritdoc />
        async Task<IReadOnlyCollection<ICustomSticker>> IGuild.GetStickersAsync(CacheMode mode, RequestOptions? options)
        {
            if (mode != CacheMode.AllowDownload)
                return Stickers;

            return await GetStickersAsync(options).ConfigureAwait(false);
        }
        /// <inheritdoc />
        async Task<ICustomSticker> IGuild.ModifyStickerAsync(ulong id, Action<StickerProperties> func, RequestOptions? options)
            => await ModifyStickerAsync(id, func, options).ConfigureAwait(false);
        /// <inheritdoc />
        Task IGuild.DeleteStickerAsync(ICustomSticker sticker, RequestOptions? options)
        {
            if (sticker is null)
                return Task.FromException(new ArgumentNullException(nameof(sticker)));

            return DeleteStickerAsync(sticker.Id, options);
        }
        /// <inheritdoc />
        async Task<IApplicationCommand> IGuild.CreateApplicationCommandAsync(ApplicationCommandProperties properties, RequestOptions? options)
            => await CreateApplicationCommandAsync(properties, options).ConfigureAwait(false);
        /// <inheritdoc />
        async Task<IReadOnlyCollection<IApplicationCommand>> IGuild.BulkOverwriteApplicationCommandsAsync(ApplicationCommandProperties[] properties,
            RequestOptions? options)
            => await BulkOverwriteApplicationCommandsAsync(properties, options).ConfigureAwait(false);
        /// <inheritdoc />
        async Task<IApplicationCommand?> IGuild.GetApplicationCommandAsync(ulong id, CacheMode mode, RequestOptions? options)
        {
            if (mode == CacheMode.AllowDownload)
            {
                return await GetApplicationCommandAsync(id, options).ConfigureAwait(false);;
            }
            else
                return null;
        }

        /// <inheritdoc/>
        async Task<IApplicationCommand> IGuild.ModifyApplicationCommandAsync<TArg>(ulong id, Action<TArg> func, RequestOptions? options)
            => await ModifyApplicationCommandAsync(id, func, options).ConfigureAwait(false);

        /// <inheritdoc/>
        async Task<IGuildScheduledEvent> IGuild.ModifyEventAsync(ulong id, Action<GuildScheduledEventsProperties> func, RequestOptions? options)
            => await ModifyEventAsync(id, func, options).ConfigureAwait(false);

        /// <inheritdoc/>
        public Task<WelcomeScreen?> GetWelcomeScreenAsync(RequestOptions? options = null)
            => GuildHelper.GetWelcomeScreenAsync(this, Discord, options);

        /// <inheritdoc/>
        public Task<WelcomeScreen?> ModifyWelcomeScreenAsync(bool enabled, WelcomeScreenChannelProperties[] channels, string? description = null, RequestOptions? options = null)
            => GuildHelper.ModifyWelcomeScreenAsync(enabled, description, channels, this, Discord, options);


        /// <inheritdoc/>
        async Task<IAutoModRule?> IGuild.GetAutoModRuleAsync(ulong ruleId, RequestOptions? options)
            => await GetAutoModRuleAsync(ruleId, options).ConfigureAwait(false);

        /// <inheritdoc/>
        async Task<IAutoModRule[]> IGuild.GetAutoModRulesAsync(RequestOptions? options)
            => await GetAutoModRulesAsync(options).ConfigureAwait(false);

        /// <inheritdoc/>
        async Task<IAutoModRule> IGuild.CreateAutoModRuleAsync(Action<AutoModRuleProperties> props, RequestOptions? options)
            => await CreateAutoModRuleAsync(props, options).ConfigureAwait(false);
        
        /// <inheritdoc/>
        async Task<IGuildOnboarding> IGuild.GetOnboardingAsync(RequestOptions? options)
            => await GetOnboardingAsync(options).ConfigureAwait(false);

        /// <inheritdoc/>
        async Task<IGuildOnboarding> IGuild.ModifyOnboardingAsync(Action<GuildOnboardingProperties> props, RequestOptions? options)
            => await ModifyOnboardingAsync(props, options).ConfigureAwait(false);

        #endregion
    }
}
