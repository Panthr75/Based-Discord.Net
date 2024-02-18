namespace Discord.OAuth;

/// <summary>
/// The credentials for an <see cref="OAuthClient"/>.
/// <para>
/// These can be found at
/// <c>https://discord.com/developers/applications/{application-id}/oauth2/general</c>,
/// or in the <c>OAuth2 -&gt; General</c> section.
/// </para>
/// </summary>
public sealed class OAuthClientCredentials
{
    /// <summary>
    /// The ID of the oauth client.
    /// application.
    /// </summary>
    public ulong ClientId { get; set; }

    /// <summary>
    /// The secret key of the oauth client.
    /// </summary>
    public string ClientSecret { get; set; } = null!;

    internal void Validate()
    {
        Preconditions.GreaterThan(0UL, this.ClientId, nameof(this.ClientId));
        Preconditions.NotNullOrEmpty(this.ClientSecret, nameof(this.ClientSecret));
    }
}
