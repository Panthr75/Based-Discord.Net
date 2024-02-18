using Discord.Rest;
using System;
using System.Threading.Tasks;

namespace Discord.OAuth;

/// <summary>
/// Represents a configuration class for
/// <see cref="OAuthClient"/>.
/// </summary>
public sealed class OAuthClientConfig : DiscordRestConfig
{
    /// <summary>
    /// Whether or not access tokens should be automatically 
    /// refreshed. If they are,
    /// <see cref="TokenRefreshedCallback"/> (if specified)
    /// will be called.
    /// </summary>
    /// <remarks>
    /// Defaults to <see langword="true"/>.
    /// </remarks>
    public bool AutoRefreshTokens { get; set; } = true;

    /// <summary>
    /// The callback for when an oauth access client has it's
    /// tokens refreshed.
    /// </summary>
    /// <remarks>
    /// Defaults to <see langword="null"/>.
    /// </remarks>
    public Func<OAuthAccessClient, Task>? TokenRefreshedCallback { get; set; } = null;
}
