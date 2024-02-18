using Discord.API;
using Discord.API.OAuth;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Discord.OAuth;

/// <summary>
/// A client for handling discord oauth.
/// </summary>
public sealed class OAuthClient : IDisposable, IAsyncDisposable
{
    private readonly Func<OAuthAccessClient, Task>? refreshTokenCallback;
    private readonly bool autoRefreshTokens;

    /// <inheritdoc/>
    public OAuthClient() : this(new OAuthClientConfig())
    { }

    /// <summary>
    /// Initializes a new <see cref="OAuthClient"/> with the
    /// provided configuration.
    /// </summary>
    /// <param name="config">
    /// The configuration to be used with the client.
    /// </param>
    public OAuthClient(OAuthClientConfig config)
        : this(config, OAuthClient.CreateAPIClient(config))
    { }

    internal OAuthClient(OAuthClientConfig config, DiscordOAuthApiClient apiClient)
    {
        this.ApiClient = apiClient;
        this.autoRefreshTokens = config.AutoRefreshTokens;
        this.refreshTokenCallback = config.TokenRefreshedCallback;
    }

    private static DiscordOAuthApiClient CreateAPIClient(OAuthClientConfig config)
    {
        return new DiscordOAuthApiClient(
            restClientProvider: config.RestClientProvider,
            userAgent: OAuthClientConfig.UserAgent,
            defaultRetryMode: config.DefaultRetryMode,
            serializerOptions: Discord.Net.Converters.DiscordJsonOptionsFactory.CreateDefaultOptions(),
            useSystemClock: config.UseSystemClock,
            defaultRatelimitCallback: config.DefaultRatelimitCallback);
    }

    internal DiscordOAuthApiClient ApiClient { get; }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.ApiClient.Dispose();
    }

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        return this.ApiClient.DisposeAsync();
    }

    /// <summary>
    /// Logs this client in with the provided oauth
    /// credentials.
    /// </summary>
    /// <param name="credentials">
    /// The oauth credentials to login with.
    /// </param>
    /// <param name="options">
    /// The options for sending the request.
    /// </param>
    /// <returns>
    /// An awaitable task.
    /// </returns>
    public async Task LoginAsync(OAuthClientCredentials credentials, RequestOptions? options = null)
    {
        Preconditions.NotNull(credentials, nameof(credentials));
        credentials.Validate();
        await this.ApiClient.LoginAsync(TokenType.OAuthClient, Convert.ToBase64String(Encoding.ASCII.GetBytes($"{credentials.ClientId}:{credentials.ClientSecret}")), options).ConfigureAwait(false);
    }

    internal Task ValidateAccessTokenAsync(OAuthAccessClient client, RequestOptions? options)
    {
        if (!client.AccessToken.IsExpired)
            return Task.CompletedTask;
        else if (this.autoRefreshTokens)
            return this.RefreshAccessTokenAsync(client, options);
        else
            return Task.FromException(new InvalidOperationException("Access token has expired!"));

    }

    internal async Task RefreshAccessTokenAsync(OAuthAccessClient client, RequestOptions? options)
    {
        if (client.RefreshToken is null)
            throw new InvalidOperationException("Cannot refresh access token with null refresh token.");

        OAuthToken oauthToken = await this.ApiClient.RefreshTokenAsync(client.RefreshToken, options).ConfigureAwait(false);

        client.UpdateToken(oauthToken);

        if (this.refreshTokenCallback is null)
            return;

        _ = Task.Run(async () =>
        {
            await this.refreshTokenCallback.Invoke(client);
        });
    }

    /// <summary>
    /// Creates an <see cref="OAuthAccessClient"/> from the
    /// specified authorization code.
    /// </summary>
    /// <param name="code">
    /// The authorization code.
    /// </param>
    /// <param name="redirectUri">
    /// The redirect URI that created the code.
    /// </param>
    /// <param name="identityOptions">
    /// The options for identifying the client.
    /// </param>
    /// <param name="options">
    /// The options for sending the request.
    /// </param>
    /// <returns>
    /// An awaitable task that when resolved contains a
    /// reference to the newly created/cached
    /// <see cref="OAuthAccessClient"/>.
    /// </returns>
    public async Task<OAuthAccessClient> CreateClientFromCodeAsync(
        string code,
        string redirectUri,
        OAuthIdentityOptions? identityOptions = null,
        RequestOptions? options = null)
    {
        OAuthToken token = await this.ApiClient.AuthorizeCodeAsync(code, redirectUri, options).ConfigureAwait(false);

        if (identityOptions is null || 
            !identityOptions.Enabled ||
            !token.Scope.Contains(OAuthScopes.Identify))
        {
            return new OAuthAccessClient(this, token);
        }

        User model = await this.ApiClient.GetMyUserAsync($"Bearer {token.AccessToken}", options).ConfigureAwait(false);

        OAuthAccessClient? client = identityOptions.TryGet?.Invoke(model.Id);
        if (client is not null)
        {
            client.UpdateToken(token);
            client.UpdateUser(model);
            return client;
        }

        if (identityOptions.TryGetAsync is not null)
        {
            client = await identityOptions.TryGetAsync(model.Id).ConfigureAwait(false);

            if (client is not null)
            {
                client.UpdateToken(token);
                client.UpdateUser(model);
                return client;
            }
        }

        client = new OAuthAccessClient(this, token, model);

        identityOptions.CacheClient?.Invoke(client);
        if (identityOptions.CacheClientAsync is not null)
        {
            await identityOptions.CacheClientAsync(client).ConfigureAwait(false);
        }
        return client;
    }

    /// <summary>
    /// Creates an <see cref="OAuthAccessClient"/> from the
    /// specified token info.
    /// </summary>
    /// <param name="accessToken">
    /// The access token.
    /// </param>
    /// <param name="accessTokenExpiry">
    /// The access token expiry.
    /// </param>
    /// <param name="scopes">
    /// The scopes allowed for the access token.
    /// </param>
    /// <param name="refreshToken">
    /// The optional refresh token.
    /// </param>
    /// <param name="identityOptions">
    /// The options for identifying the client.
    /// </param>
    /// <param name="options">
    /// The options for sending the request.
    /// </param>
    /// <returns>
    /// An awaitable task that when resolved contains a
    /// reference to the newly created/cached
    /// <see cref="OAuthAccessClient"/>.
    /// </returns>
    public async Task<OAuthAccessClient> CreateClientFromTokensAsync(
        string accessToken,
        DateTimeOffset accessTokenExpiry,
        OAuthScopeCollection scopes,
        string? refreshToken = null,
        OAuthIdentityOptions? identityOptions = null,
        RequestOptions? options = null)
    {
        if (identityOptions is null ||
            !identityOptions.Enabled ||
            !scopes.Identify)
        {
            return new OAuthAccessClient(this,
                accessToken,
                accessTokenExpiry,
                scopes,
                refreshToken);
        }

        User model = await this.ApiClient.GetMyUserAsync($"Bearer {accessToken}", options).ConfigureAwait(false);

        OAuthAccessClient? client = identityOptions.TryGet?.Invoke(model.Id);
        if (client is not null)
        {
            client.UpdateToken(accessToken, accessTokenExpiry, scopes, refreshToken);
            client.UpdateUser(model);
            return client;
        }

        if (identityOptions.TryGetAsync is not null)
        {
            client = await identityOptions.TryGetAsync(model.Id).ConfigureAwait(false);

            if (client is not null)
            {
                client.UpdateToken(accessToken, accessTokenExpiry, scopes, refreshToken);
                client.UpdateUser(model);
                return client;
            }
        }

        client = new OAuthAccessClient(this,
            accessToken,
            accessTokenExpiry,
            scopes,
            refreshToken,
            model);

        identityOptions.CacheClient?.Invoke(client);
        if (identityOptions.CacheClientAsync is not null)
        {
            await identityOptions.CacheClientAsync(client).ConfigureAwait(false);
        }
        return client;
    }
}
