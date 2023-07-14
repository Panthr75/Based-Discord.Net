using System.Collections.Generic;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IApplication"/>
    /// </summary>
    public interface IApplicationShim : IApplication, ISnowflakeShimEntity
    {
        /// <inheritdoc cref="IApplication.Name"/>
        new string Name { get; set; }
        /// <inheritdoc cref="IApplication.Description"/>
        new string Description { get; set; }
        /// <inheritdoc cref="IApplication.RPCOrigins"/>
        new ICollection<string> RPCOrigins { get; }
        /// <inheritdoc cref="IApplication.Flags"/>
        new ApplicationFlags Flags { get; set; }
        /// <inheritdoc cref="IApplication.InstallParams"/>
        new ApplicationInstallParamsShim? InstallParams { get; set; }
        /// <inheritdoc cref="IApplication.Tags"/>
        new ICollection<string> Tags { get; }
        /// <inheritdoc cref="IApplication.IsBotPublic"/>
        new bool? IsBotPublic { get; set; }
        /// <inheritdoc cref="IApplication.BotRequiresCodeGrant"/>
        new bool? BotRequiresCodeGrant { get; set; }
        /// <inheritdoc cref="IApplication.Team"/>
        new ITeamShim? Team { get; set; }
        /// <inheritdoc cref="IApplication.Owner"/>
        new IUserShim? Owner { get; set; }
        /// <inheritdoc cref="IApplication.TermsOfService"/>
        new string? TermsOfService { get; set; }
        /// <inheritdoc cref="IApplication.PrivacyPolicy"/>
        new string? PrivacyPolicy { get; set; }

        /// <inheritdoc cref="IApplication.CustomInstallUrl"/>
        new string? CustomInstallUrl { get; set; }

        /// <inheritdoc cref="IApplication.RoleConnectionsVerificationUrl"/>
        new string? RoleConnectionsVerificationUrl { get; set; }

        /// <inheritdoc cref="IApplication.VerifyKey"/>
        new string VerifyKey { get; set; }

        /// <inheritdoc cref="IApplication.Guild"/>
        new PartialGuildShim? Guild { get; set; }

        /// <inheritdoc cref="IApplication.RedirectUris"/>
        new ICollection<string> RedirectUris { get; }

        /// <inheritdoc cref="IApplication.InteractionsEndpointUrl"/>
        new string? InteractionsEndpointUrl { get; set; }

        /// <inheritdoc cref="IApplication.ApproximateGuildCount"/>
        new int? ApproximateGuildCount { get; set; }
    }
}
