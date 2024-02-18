using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Discord.OAuth;

/// <summary>
/// A representation of an OAuth User.
/// </summary>
public abstract class OAuthUser : IUser
{
    internal OAuthUser(ulong id)
    {
        this.Id = id;
    }

    /// <inheritdoc/>
    public ulong Id { get; }
    /// <inheritdoc/>
    public DateTimeOffset CreatedAt => SnowflakeUtils.FromSnowflake(this.Id);

    /// <summary>
    /// The access client that created this user.
    /// </summary>
    public virtual OAuthAccessClient AccessClient => this.GlobalUser.AccessClient;
    internal abstract OAuthGlobalUser GlobalUser { get; set; }

    /// <inheritdoc/>
    public virtual bool IsBot
    {
        get => this.GlobalUser.IsBot;
        internal set => this.GlobalUser.IsBot = value;
    }

    /// <inheritdoc/>
    public virtual string Username
    {
        get => this.GlobalUser.Username;
        internal set => this.GlobalUser.Username = value;
    }

    /// <inheritdoc/>
    public virtual ushort DiscriminatorValue
    {
        get => this.GlobalUser.DiscriminatorValue;
        internal set => this.GlobalUser.DiscriminatorValue = value;
    }

    /// <inheritdoc/>
    public virtual string? AvatarId
    {
        get => this.GlobalUser.AvatarId;
        internal set => this.GlobalUser.AvatarId = value;
    }

    /// <inheritdoc/>
    public abstract bool IsWebhook { get; }

    /// <inheritdoc/>
    public virtual UserProperties? PublicFlags
    {
        get => this.GlobalUser.PublicFlags;
        internal set => this.GlobalUser.PublicFlags = value;
    }

    /// <inheritdoc/>
    public virtual string? GlobalName
    {
        get => this.GlobalUser.GlobalName;
        internal set => this.GlobalUser.GlobalName = value;
    }

    /// <inheritdoc/>
    public virtual string? AvatarDecorationHash
    {
        get => this.GlobalUser.AvatarDecorationHash;
        internal set => this.GlobalUser.AvatarDecorationHash = value;
    }

    /// <inheritdoc/>
    public virtual ulong? AvatarDecorationSkuId
    {
        get => this.GlobalUser.AvatarDecorationSkuId;
        internal set => this.GlobalUser.AvatarDecorationSkuId = value;
    }

    /// <inheritdoc/>
    public virtual string? Pronouns
    {
        get => this.GlobalUser.Pronouns;
        internal set => this.GlobalUser.Pronouns = value;
    }

    /// <summary>
    /// The flags of this user.
    /// </summary>
    public virtual UserProperties? Flags
    {
        get => this.GlobalUser.Flags;
        internal set => this.GlobalUser.Flags = value;
    }

    /// <summary>
    /// Gets the user's chosen language option.
    /// </summary>
    public virtual string? Locale
    {
        get => this.GlobalUser.Locale;
        internal set => this.GlobalUser.Locale = value;
    }

    /// <summary>
    /// Gets the color of the user's banner.
    /// </summary>
    public virtual Color? AccentColor
    {
        get => this.GlobalUser.AccentColor;
        internal set => this.GlobalUser.AccentColor = value;
    }

    /// <summary>
    /// Gets the identifier of this user's banner, if they have
    /// one.
    /// </summary>
    public virtual string? BannerId
    {
        get => this.GlobalUser.BannerId;
        internal set => this.GlobalUser.BannerId = value;
    }

    /// <summary>
    /// The type of premium subscription the user is subscribed
    /// to.
    /// </summary>
    public virtual PremiumType PremiumType
    {
        get => this.GlobalUser.PremiumType;
        internal set => this.GlobalUser.PremiumType = value;
    }

    /// <inheritdoc/>
    public string Mention => MentionUtils.MentionUser(this.Id);

    /// <inheritdoc/>
    public string Discriminator => this.DiscriminatorValue.ToString("D4");

    /// <inheritdoc/>
    public virtual UserStatus Status => UserStatus.Offline;

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<ClientType> ActiveClients => ImmutableHashSet<ClientType>.Empty;

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<IActivity> Activities => ImmutableList<IActivity>.Empty;

    /// <inheritdoc/>
    public string? GetAvatarUrl(ImageFormat format = ImageFormat.Auto, ushort size = 128)
        => CDN.GetUserAvatarUrl(this.Id, this.AvatarId, size, format);

    /// <inheritdoc/>
    public string GetDefaultAvatarUrl()
    {
        ushort discriminator = this.DiscriminatorValue;
        if (discriminator != 0)
            return CDN.GetDefaultUserAvatarUrl(discriminator);
        else
            return CDN.GetDefaultUserAvatarUrl(this.Id);
    }

    /// <inheritdoc/>
    public string GetDisplayAvatarUrl(ImageFormat format = ImageFormat.Auto, ushort size = 128)
        => this.GetAvatarUrl(format, size) ?? this.GetDefaultAvatarUrl();

    /// <inheritdoc/>
    public string? GetAvatarDecorationUrl()
    {
        string? avatarDecorationHash = this.AvatarDecorationHash;
        if (avatarDecorationHash is null)
            return null;
        else
            return CDN.GetAvatarDecorationUrl(avatarDecorationHash);
    }

    /// <inheritdoc/>
    public string? GetBannerUrl(ImageFormat format = ImageFormat.Auto, ushort size = 256)
        => CDN.GetUserBannerUrl(this.Id, this.BannerId, size, format);

    /// <summary>
    /// Attempts to fetch this user from the specified discord
    /// client.
    /// </summary>
    /// <param name="client">
    /// The client to fetch the user from.
    /// </param>
    /// <param name="mode">
    /// The mode for caching.
    /// </param>
    /// <param name="options">
    /// The options for sending the request.
    /// </param>
    /// <returns>
    /// A task containing the result of whether or not the user
    /// was fetched successfully.
    /// </returns>
    public virtual Task<bool> TryFetchUserFromClientAsync(IDiscordClient client, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null)
    {
        return this.GlobalUser.TryFetchUserFromClientAsync(client, mode, options);
    }

    /// <summary>
    /// Attempts to fetch this user from the specified discord
    /// client.
    /// </summary>
    /// <param name="client">
    /// The client to fetch the user from.
    /// </param>
    /// <param name="mode">
    /// The mode for caching.
    /// </param>
    /// <param name="options">
    /// The options for sending the request.
    /// </param>
    /// <returns>
    /// A task containing the result of whether or not the user
    /// was fetched successfully.
    /// </returns>
    public virtual Task<bool> TryFetchUserFromClientAsync(DiscordRestClient client, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null)
    {
        return this.GlobalUser.TryFetchUserFromClientAsync(client, mode, options);
    }

    /// <summary>
    /// Attempts to fetch this user from the specified discord
    /// client.
    /// </summary>
    /// <param name="client">
    /// The client to fetch the user from.
    /// </param>
    /// <param name="mode">
    /// The mode for caching.
    /// </param>
    /// <param name="options">
    /// The options for sending the request.
    /// </param>
    /// <returns>
    /// A task containing the result of whether or not the user
    /// was fetched successfully.
    /// </returns>
    public virtual Task<bool> TryFetchUserFromClientAsync(BaseSocketClient client, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null)
    {
        return this.GlobalUser.TryFetchUserFromClientAsync(client, mode, options);
    }

    Task<IDMChannel> IUser.CreateDMChannelAsync(RequestOptions? options)
        => Task.FromException<IDMChannel>(new NotSupportedException());
}
