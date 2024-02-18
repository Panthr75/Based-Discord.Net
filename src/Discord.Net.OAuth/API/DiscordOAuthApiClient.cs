using Discord.API.OAuth;
using Discord.Net.Rest;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Discord.API;

internal sealed class DiscordOAuthApiClient : DiscordRestApiClient
{
    public DiscordOAuthApiClient(
        RestClientProvider restClientProvider,
        string userAgent,
        RetryMode defaultRetryMode = RetryMode.AlwaysRetry,
        JsonSerializerOptions? serializerOptions = null,
        bool useSystemClock = true,
        Func<IRateLimitInfo, Task>? defaultRatelimitCallback = null)
        : base(restClientProvider, userAgent, defaultRetryMode, serializerOptions, useSystemClock, defaultRatelimitCallback)
    { }

    public Task<OAuthToken> RefreshTokenAsync(string refreshToken, RequestOptions? options)
    {
        options = RequestOptions.CreateOrClone(options);

        KeyValuePair<string, string>[] body = new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("refresh_token", refreshToken)
        };

        return this.SendWwwFormUrlEncodedAsync<OAuthToken>("GET", () => "oauth2/token", body, new BucketIds(), options: options);
    }

    public Task<OAuthToken> AuthorizeCodeAsync(string code, string redirectUri, RequestOptions? options)
    {
        Preconditions.NotNull(code, nameof(code));
        Preconditions.NotNull(redirectUri, nameof(redirectUri));
        options = RequestOptions.CreateOrClone(options);

        KeyValuePair<string, string>[] body = new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("code", code),
            new KeyValuePair<string, string>("redirect_uri", redirectUri)
        };

        return this.SendWwwFormUrlEncodedAsync<OAuthToken>("GET", () => "oauth2/token", body, new BucketIds(), options: options);
    }

    public Task<User> GetMyUserAsync(string authorization, RequestOptions? options)
    {
        options = RequestOptions.CreateOrClone(options);

        options.RequestHeaders["authorization"] = new string[] { authorization };

        return this.SendAsync<User>("GET", () => "users/@me", new BucketIds(), options: options);
    }
}
