using Discord.API;
using Discord.API.OAuth;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Discord.OAuth;

/// <summary>
/// A client that provides access to an oauth instance via an
/// access token.
/// </summary>
public sealed class OAuthAccessClient
{
    private ImmutableDictionary<ulong, OAuthGuild> guildsById;
    private ImmutableArray<OAuthGuild> guilds;
    private OAuthGlobalUser? currentUser;

    internal OAuthAccessClient(OAuthClient client, OAuthToken token)
    {
        this.guildsById = ImmutableDictionary<ulong, OAuthGuild>.Empty;
        this.guilds = ImmutableArray<OAuthGuild>.Empty;
        this.OAuthClient = client;
        this.RefreshToken = string.Empty;
        this.Scopes = OAuthScopeCollection.None;
        this.ApiClient = new DiscordOAuthApiUserClient(client.ApiClient, string.Empty);
        this.UpdateToken(token);
    }

    internal OAuthAccessClient(OAuthClient client, OAuthToken token, User model)
        : this(client, token)
    {
        this.currentUser = new OAuthGlobalUser(model.Id, this);
        this.currentUser.Update(model);
    }

    internal OAuthAccessClient(
        OAuthClient client,
        string accessToken,
        DateTimeOffset accessTokenExpiry,
        OAuthScopeCollection scopes,
        string? refreshToken)
    {
        this.guildsById = ImmutableDictionary<ulong, OAuthGuild>.Empty;
        this.guilds = ImmutableArray<OAuthGuild>.Empty;
        this.OAuthClient = client;
        this.RefreshToken = refreshToken;
        this.AccessToken = new OAuthAccessToken(accessToken, accessTokenExpiry);
        this.Scopes = scopes;
        this.ApiClient = new DiscordOAuthApiUserClient(client.ApiClient, accessToken);
    }

    internal OAuthAccessClient(
        OAuthClient client,
        string accessToken,
        DateTimeOffset accessTokenExpiry,
        OAuthScopeCollection scopes,
        string? refreshToken,
        User model) : this(
            client: client,
            accessToken: accessToken,
            accessTokenExpiry: accessTokenExpiry,
            scopes: scopes,
            refreshToken: refreshToken)
    {
        this.currentUser = new OAuthGlobalUser(model.Id, this);
        this.currentUser.Update(model);
    }

    /// <summary>
    /// The oauth client that created this instance.
    /// </summary>
    public OAuthClient OAuthClient { get; }

    /// <summary>
    /// The access token.
    /// </summary>
    public OAuthAccessToken AccessToken { get; private set; }

    /// <summary>
    /// The optional refresh token.
    /// </summary>
    public string? RefreshToken { get; private set; }

    /// <summary>
    /// The scopes of <see cref="AccessToken"/>.
    /// </summary>
    public OAuthScopeCollection Scopes { get; private set; }

    /// <summary>
    /// The guilds the oauth user is in. Only populated when
    /// calling
    /// <see cref="GetMyGuildsAsync(RequestOptions?)"/>.
    /// </summary>
    public IReadOnlyCollection<OAuthGuild> Guilds => this.guilds;

    /// <summary>
    /// The current user. Can be null if not fetched from
    /// <see cref="GetMyCurrentUserAsync(RequestOptions?)"/>,
    /// or if fetching the identity wasn't specified/enabled
    /// when creating this instance.
    /// </summary>
    public OAuthUser? CurrentUser => this.currentUser;

    internal DiscordOAuthApiUserClient ApiClient { get; }

    internal void UpdateToken(OAuthToken token)
    {
        this.AccessToken = new OAuthAccessToken(token.AccessToken, DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn));
        this.RefreshToken = token.RefreshToken;
        this.Scopes = OAuthScopeCollection.Parse(token.Scope);
        this.ApiClient.AccessToken = token.AccessToken;
    }

    internal void UpdateToken(
        string accessToken,
        DateTimeOffset accessTokenExpiry,
        OAuthScopeCollection scopes,
        string? refreshToken)
    {
        this.AccessToken = new(accessToken, accessTokenExpiry);
        this.Scopes = scopes;
        this.RefreshToken = refreshToken;
    }

    internal void UpdateUser(User model)
    {
        this.currentUser ??= new OAuthGlobalUser(model.Id, this);
        this.currentUser.Update(model);
    }

    /// <summary>
    /// Gets the current oauth user. Requires the
    /// <c><see cref="OAuthScopes.Identify"/></c> scope.
    /// </summary>
    /// <param name="options">
    /// The options for sending the request.
    /// </param>
    /// <returns>
    /// An awaitable task of the oauth user.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if missing the
    /// <see cref="OAuthScopes.Identify"/> scope, or the access
    /// token has expired and cannot be refreshed.
    /// </exception>
    public async Task<OAuthUser> GetMyCurrentUserAsync(RequestOptions? options = null)
    {
        await this.OAuthClient.ValidateAccessTokenAsync(this, options).ConfigureAwait(false);
        if (!this.Scopes.Identify)
            throw new InvalidOperationException($"Missing '{OAuthScopes.Identify}' scope!");

        User model = await this.ApiClient.GetMyUserAsync(options);
        if (this.currentUser is null)
        {
            this.currentUser = new OAuthGlobalUser(model.Id, this);
            this.currentUser.Update(model);

            foreach (OAuthGuild guild in this.guildsById.Values)
            {
                guild.RefreshCurrentUser();
            }
        }
        else
            this.currentUser.Update(model);

        return this.currentUser;
    }

    /// <summary>
    /// Gets the guilds the oauth user is in. Requires the
    /// <c><see cref="OAuthScopes.Guilds"/></c> scope.
    /// </summary>
    /// <param name="options">
    /// The options for sending the request.
    /// </param>
    /// <returns>
    /// An awaitable task of all the guilds the oauth user is
    /// in.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if missing the
    /// <see cref="OAuthScopes.Guilds"/> scope, or the access
    /// token has expired and cannot be refreshed.
    /// </exception>
    public async Task<IReadOnlyCollection<OAuthGuild>> GetMyGuildsAsync(RequestOptions? options = null)
    {
        await this.OAuthClient.ValidateAccessTokenAsync(this, options).ConfigureAwait(false);
        if (!this.Scopes.Guilds)
            throw new InvalidOperationException($"Missing '{OAuthScopes.Guilds}' scope!");

        IReadOnlyCollection<UserGuild> userGuilds = await this.ApiClient.GetMyGuildsAsync(options).ConfigureAwait(false);

        ImmutableDictionary<ulong, OAuthGuild>.Builder mapBuilder = ImmutableDictionary.CreateBuilder<ulong, OAuthGuild>();
        ImmutableArray<OAuthGuild>.Builder builder = ImmutableArray.CreateBuilder<OAuthGuild>(userGuilds.Count);
        foreach (UserGuild userGuild in userGuilds)
        {
            if (this.guildsById.TryGetValue(userGuild.Id, out OAuthGuild? guild))
            {
                guild.Update(userGuild);
            }
            else
            {
                guild = new OAuthGuild(userGuild, this);
            }

            mapBuilder.Add(userGuild.Id, guild);
            builder.Add(guild);
        }

        this.guildsById = mapBuilder.ToImmutable();
        this.guilds = builder.ToImmutable();
        return this.guilds;
    }

    internal async Task UpdateCurrentMemberInGuildAsync(OAuthGuild guild, RequestOptions? options)
    {
        await this.OAuthClient.ValidateAccessTokenAsync(this, options).ConfigureAwait(false);
        if (!this.Scopes.ReadGuildMembers)
            throw new InvalidOperationException($"Missing '{OAuthScopes.ReadGuildMembers}' scope!");

        GuildMember model = await this.ApiClient.GetCurrentUserGuildMember(guild.Id, options).ConfigureAwait(false);

        if (this.currentUser is null)
        {
            this.currentUser = new OAuthGlobalUser(model.User.Id, this);
            this.currentUser.Update(model.User);

            foreach (OAuthGuild userGuild in this.guildsById.Values)
            {
                userGuild.RefreshCurrentUser();
            }
        }
        else
            this.currentUser.Update(model.User);

        guild.CurrentUserSelf!.Update(model);
    }
}
