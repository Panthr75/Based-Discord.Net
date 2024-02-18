using System;
using System.Runtime.CompilerServices;

namespace Discord.OAuth;

/// <summary>
/// Represents a Discord OAuth Access Token.
/// </summary>
public readonly struct OAuthAccessToken
{
    /// <summary>
    /// Initializes this Discord OAuth Access Token with the
    /// specified token and expiry.
    /// </summary>
    /// <param name="token">
    /// The oauth access token string.
    /// </param>
    /// <param name="expiresAt">
    /// The time at which <paramref name="token"/> will expire.
    /// </param>
    public OAuthAccessToken(string token, DateTimeOffset expiresAt)
    {
        this.Token = token;
        this.ExpiresAt = expiresAt;
    }

    /// <summary>
    /// The raw access token.
    /// </summary>
    public string Token { get; }

    /// <summary>
    /// The time at which this access token expires.
    /// </summary>
    public DateTimeOffset ExpiresAt { get; }

    /// <summary>
    /// Whether or not this access token has expired at the
    /// time of calling this property.
    /// </summary>
    public bool IsExpired => DateTimeOffset.UtcNow > this.ExpiresAt;

    /// <summary>
    /// Gets whether or not the access token expires at the
    /// specified time.
    /// </summary>
    /// <param name="time">
    /// The time to check.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if this access token will expire
    /// at <paramref name="time"/>; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsExpiredAt(DateTimeOffset time)
    {
        return time > this.ExpiresAt;
    }
}
