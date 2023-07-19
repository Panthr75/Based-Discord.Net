using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Discord;

/// <summary>
///     Represents install parameters for an application.
/// </summary>
public class ApplicationInstallParams
{
    /// <summary>
    ///     Gets the scopes to install this application.
    /// </summary>
    public IReadOnlyCollection<string> Scopes { get; }

    /// <summary>
    ///     Gets the default permissions to install this application
    /// </summary>
    public GuildPermission Permission { get; }

    public ApplicationInstallParams(string[] scopes, GuildPermission permission)
    {
        Preconditions.NotNull(scopes, nameof(scopes));
        Scopes = scopes
            .Where(scope => !string.IsNullOrEmpty(scope))
            .ToImmutableArray();
        Permission = permission;
    }
}
