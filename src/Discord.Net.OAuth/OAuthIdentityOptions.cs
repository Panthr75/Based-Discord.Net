using System;
using System.Threading.Tasks;

namespace Discord.OAuth;

/// <summary>
/// The options for creating an <see cref="OAuthAccessClient"/>
/// with an identity using the
/// <see cref="OAuthScopes.Identify"/> scope;
/// </summary>
public sealed class OAuthIdentityOptions
{
    /// <summary>
    /// If <see langword="true"/>, creating an
    /// <see cref="OAuthAccessClient"/> will also include
    /// getting the current oauth user.
    /// </summary>
    /// <remarks>
    /// If <see langword="false"/>, then none of the
    /// functionality in these options will work.
    /// </remarks>
    public bool Enabled { get; set; }

    /// <summary>
    /// When fetching the current oauth user, if an
    /// <see cref="OAuthAccessClient"/> is returned from
    /// calling this function (instead of null), then it will
    /// be returned instead of creating a new one.
    /// </summary>
    public Func<ulong, OAuthAccessClient?>? TryGet { get; set; }

    /// <summary>
    /// When fetching the current oauth user, if an
    /// <see cref="OAuthAccessClient"/> is returned from
    /// calling this function and awaiting the result (instead
    /// of null), then it will be returned instead of creating
    /// a new one.
    /// </summary>
    public Func<ulong, Task<OAuthAccessClient?>>? TryGetAsync { get; set; }

    /// <summary>
    /// A function that is called to cache the specified
    /// <see cref="OAuthAccessClient"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="OAuthAccessClient.CurrentUser"/> will not be
    /// <see langword="null"/>.
    /// </remarks>
    public Action<OAuthAccessClient>? CacheClient { get; set; }

    /// <summary>
    /// A function that is called to cache the specified
    /// <see cref="OAuthAccessClient"/> asynchronously.
    /// </summary>
    public Func<OAuthAccessClient, Task>? CacheClientAsync { get; set; }
}
