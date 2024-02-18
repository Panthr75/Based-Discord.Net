using Discord.API;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Discord.OAuth;

internal sealed class OAuthGlobalUser : OAuthUser
{
    public OAuthGlobalUser(ulong id, OAuthAccessClient accessClient)
        : base(id)
    {
        this.AccessClient = accessClient;
        this.Username = string.Empty;
    }

    public sealed override OAuthAccessClient AccessClient { get; }

    internal sealed override OAuthGlobalUser GlobalUser
    {
        get => this;
        set => throw new NotImplementedException();
    }

    public sealed override bool IsWebhook => false;

    public sealed override bool IsBot { get; internal set; }
    public sealed override string Username { get; internal set; }
    public sealed override ushort DiscriminatorValue { get; internal set; }
    public sealed override string? AvatarId { get; internal set; }
    public sealed override UserProperties? PublicFlags { get; internal set; }
    public sealed override string? GlobalName { get; internal set; }
    public sealed override string? AvatarDecorationHash { get; internal set; }
    public sealed override ulong? AvatarDecorationSkuId { get; internal set; }
    public sealed override string? Pronouns { get; internal set; }

    public sealed override UserProperties? Flags { get; internal set; }
    public sealed override string? Locale { get; internal set; }
    public sealed override Color? AccentColor { get; internal set; }
    public sealed override string? BannerId { get; internal set; }
    public sealed override PremiumType PremiumType { get; internal set; }

    internal void Update(User model)
    {
        if (model.Bot.IsSpecified)
            this.IsBot = model.Bot.Value;
        if (model.Username.IsSpecified)
            this.Username = model.Username.Value;
        if (model.Discriminator.IsSpecified)
            this.DiscriminatorValue = ushort.Parse(model.Discriminator.Value ?? "0", NumberStyles.None, CultureInfo.InvariantCulture);
        if (model.Avatar.IsSpecified)
            this.AvatarId = model.Avatar.Value;
        if (model.PublicFlags.IsSpecified)
            this.PublicFlags = model.PublicFlags.Value;
        if (model.GlobalName.IsSpecified)
            this.GlobalName = model.GlobalName.Value;
        if (model.AvatarDecoration.IsSpecified)
        {
            this.AvatarDecorationHash = model.AvatarDecoration.Value?.Asset;
            this.AvatarDecorationSkuId = model.AvatarDecoration.Value?.SkuId;
        }
        if (model.Pronouns.IsSpecified)
            this.Pronouns = model.Pronouns.Value;

        if (model.Flags.IsSpecified)
            this.Flags = model.Flags.Value;
        if (model.Locale.IsSpecified)
            this.Locale = model.Locale.Value;
        if (model.AccentColor.IsSpecified)
        {
            uint? accentColor = model.AccentColor.Value;
            if (accentColor.HasValue)
                this.AccentColor = new Color(accentColor.Value);
            else
                this.AccentColor = null;
        }
        if (model.Banner.IsSpecified)
            this.BannerId = model.Banner.Value;

        if (model.PremiumType.IsSpecified)
            this.PremiumType = model.PremiumType.Value;
    }

    internal void Update(SocketUser user)
    {
        this.IsBot = user.IsBot;
        this.Username = user.Username;
        this.DiscriminatorValue = user.DiscriminatorValue;
        this.AvatarId = user.AvatarId;
        this.PublicFlags = user.PublicFlags;
        this.GlobalName = user.GlobalName;
        this.AvatarDecorationHash = user.AvatarDecorationHash;
        this.AvatarDecorationSkuId = user.AvatarDecorationSkuId;
        this.Pronouns = user.Pronouns;
    }

    internal void Update(IUser user)
    {
        this.IsBot = user.IsBot;
        this.Username = user.Username;
        this.DiscriminatorValue = user.DiscriminatorValue;
        this.AvatarId = user.AvatarId;
        this.PublicFlags = user.PublicFlags;
        this.GlobalName = user.GlobalName;
        this.AvatarDecorationHash = user.AvatarDecorationHash;
        this.AvatarDecorationSkuId = user.AvatarDecorationSkuId;
        this.Pronouns = user.Pronouns;
    }

    public sealed override Task<bool> TryFetchUserFromClientAsync(IDiscordClient client, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null)
    {
        if (client is DiscordRestClient restClient)
            return this.TryFetchUserFromClientAsync(restClient, mode, options);
        else if (client is BaseSocketClient socketClient)
            return this.TryFetchUserFromClientAsync(socketClient, mode, options);
        else
            return this.TryFetchUserFromUnknownClientAsync(client, mode, options);

    }

    private async Task<bool> TryFetchUserFromUnknownClientAsync(IDiscordClient client, CacheMode mode, RequestOptions? options)
    {
        IUser? user = await client.GetUserAsync(this.Id, mode, options).ConfigureAwait(false);
        if (user is null)
            return false;

        this.Update(user);
        return true;
    }

    public sealed override async Task<bool> TryFetchUserFromClientAsync(DiscordRestClient client, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null)
    {
        if (mode == CacheMode.CacheOnly)
            return false;

        User? model = await client.ApiClient.GetUserAsync(this.Id, options).ConfigureAwait(false);
        if (model is null)
            return false;

        this.Update(model);
        return true;
    }

    public sealed override async Task<bool> TryFetchUserFromClientAsync(BaseSocketClient client, CacheMode mode = CacheMode.CacheOnly, RequestOptions? options = null)
    {
        SocketUser? user = client.GetUser(this.Id);

        if (user is not null)
        {
            this.Update(user);
            return true;
        }

        if (mode == CacheMode.CacheOnly)
            return false;

        User? model = await client.Rest.ApiClient.GetUserAsync(this.Id, options).ConfigureAwait(false);
        if (model is null)
            return false;

        this.Update(model);
        return true;
    }
}
