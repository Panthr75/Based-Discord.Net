using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord.API;

internal sealed class DiscordOAuthApiUserClient
{
    public DiscordOAuthApiUserClient(DiscordOAuthApiClient oauthApiClient, string accessToken)
    {
        this.OAuthApiClient = oauthApiClient;
        this.Authorization = string.Empty;
        this.AccessToken = accessToken;
    }

    private string Authorization { get; set; }
    public string AccessToken
    {
        set
        {
            this.Authorization = $"Basic {value}";
        }
    }
    public DiscordOAuthApiClient OAuthApiClient { get; }

    public Task<User> GetMyUserAsync(RequestOptions? options)
    {
        options = RequestOptions.CreateOrClone(options);

        options.RequestHeaders["authorization"] = new string[] { this.Authorization };

        return this.OAuthApiClient.SendAsync<User>("GET", () => "users/@me", new DiscordRestApiClient.BucketIds(), options: options);
    }

    public Task<IReadOnlyCollection<UserGuild>> GetMyGuildsAsync(RequestOptions? options)
    {
        options = RequestOptions.CreateOrClone(options);

        options.RequestHeaders["authorization"] = new string[] { this.Authorization };

        return this.OAuthApiClient.SendAsync<IReadOnlyCollection<UserGuild>>("GET", () => "users/@me/guilds?with_counts=true", new DiscordRestApiClient.BucketIds(), options: options);
    }

    public Task<GuildMember> GetCurrentUserGuildMember(ulong guildId, RequestOptions? options)
    {
        options = RequestOptions.CreateOrClone(options);

        options.RequestHeaders["authorization"] = new string[] { this.Authorization };

        return this.OAuthApiClient.SendAsync<GuildMember>("GET", () => $"users/@me/guilds/{guildId}/member", new DiscordRestApiClient.BucketIds(), options: options);
    }
}
