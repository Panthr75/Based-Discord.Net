using System.Runtime.CompilerServices;

namespace Discord.OAuth;

/// <summary>
/// A static utility class containing constant values of all
/// known scopes.
/// </summary>
public static class OAuthScopes
{
    /// <summary>
    /// Allows your app to fetch data from a user's "Now
    /// Playing/Recently Played" list - not currently available
    /// for apps.
    /// </summary>
    public const string ReadActivities = "activities.read";

    /// <summary>
    /// Allows your app to update a user's activity - not
    /// currently available for apps (NOT REQUIRED FOR GAMESDK
    /// ACTIVITY MANAGER)
    /// </summary>
    public const string WriteActivities = "activities.write";

    /// <summary>
    /// Allows your app to read build data for a user's
    /// applications.
    /// </summary>
    public const string ReadApplicationBuilds = "applications.builds.read";

    /// <summary>
    /// Allows your app to upload/update builds for a user's
    /// applications - requires Discord approval.
    /// </summary>
    public const string UploadApplicationBuilds = "applications.builds.upload";

    /// <summary>
    /// Allows your app to add commands to a guild - included by
    /// default with the bot scope.
    /// </summary>
    public const string ApplicationCommands = "applications.commands";

    /// <summary>
    /// Allows your app to update its commands using a Bearer
    /// token - client credentials grant only.
    /// </summary>
    public const string UpdateApplicationCommands = "applications.commands.update";

    /// <summary>
    /// Allows your app to update permissions for its commands in
    /// a guild a user has permissions to.
    /// </summary>
    public const string UpdateApplicationCommandPermissions = "applications.commands.permissions.update";

    /// <summary>
    /// Allows your app to read entitlements for a user's
    /// applications.
    /// </summary>
    public const string ApplicationEntitlements = "applications.entitlements";

    /// <summary>
    /// Allows your app to read and update store data (SKUs,
    /// store listings, achievements, etc.) for a user's
    /// applications.
    /// </summary>
    public const string UpdateApplicationStore = "applications.store.update";

    /// <summary>
    /// For oauth2 bots, this puts the bot in the user's selected
    /// guild by default.
    /// </summary>
    public const string Bot = "bot";

    /// <summary>
    /// Allows getting a user's linked third-party accounts.
    /// </summary>
    public const string Connections = "connections";

    /// <summary>
    /// Allows your app to see information about the user's DMs
    /// and group DMs - requires Discord approval.
    /// </summary>
    public const string ReadDMChannels = "dm_channels.read";

    /// <summary>
    /// Enables retrieving the email from the current user.
    /// </summary>
    public const string Email = "email";

    /// <summary>
    /// Allows your app to join users to a group dm.
    /// </summary>
    public const string JoinGroupDM = "gdm.join";

    /// <summary>
    /// Allows retrieving basic information about all of a user's
    /// guilds.
    /// </summary>
    public const string Guilds = "guilds";

    /// <summary>
    /// Allows joining users to a guild.
    /// </summary>
    public const string JoinGuilds = "guilds.join";

    /// <summary>
    /// Allows retrieving member information in a user's guild.
    /// </summary>
    public const string ReadGuildMembers = "guilds.members.read";

    /// <summary>
    /// Allows getting the current user.
    /// </summary>
    public const string Identify = "identify";

    /// <summary>
    /// For local rpc server api access, this allows you to read
    /// messages from all client channels (otherwise restricted
    /// to channels/guilds your app creates).
    /// </summary>
    public const string ReadMessages = "messages.read";

    /// <summary>
    /// Allows your app to know a user's friends and implicit
    /// relationships - requires Discord approval.
    /// </summary>
    public const string ReadRelationships = "relationships.read";

    /// <summary>
    /// Allows your app to update a user's connection and
    /// metadata for the app.
    /// </summary>
    public const string WriteRoleConnections = "role_connections.write";

    /// <summary>
    /// For local rpc server access, this allows you to control a
    /// user's local Discord client - requires Discord approval.
    /// </summary>
    public const string Rpc = "rpc";

    /// <summary>
    /// For local rpc server access, this allows you to update a
    /// user's activity - requires Discord approval.
    /// </summary>
    public const string WriteRpcActivities = "rpc.activities.write";

    /// <summary>
    /// For local rpc server access, this allows you to receive
    /// notifications pushed out to the user - requires Discord
    /// approval.
    /// </summary>
    public const string ReadRpcNotifications = "rpc.notifications.read";

    /// <summary>
    /// For local rpc server access, this allows you to read a
    /// user's voice settings and listen for voice events -
    /// requires Discord approval.
    /// </summary>
    public const string ReadRpcVoice = "rpc.voice.read";

    /// <summary>
    /// For local rpc server access, this allows you to update a
    /// user's voice settings - requires Discord approval.
    /// </summary>
    public const string WriteRpcVoice = "rpc.voice.write";

    /// <summary>
    /// Allows your app to connect to voice on user's behalf and
    /// see all the voice members - requires Discord approval.
    /// </summary>
    public const string Voice = "voice";

    /// <summary>
    /// This generates a webhook that is returned in the oauth
    /// token response for authorization code grants.
    /// </summary>
    public const string IncomingWebhook = "webhook.incoming";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string? GetNameFromIndex(int index)
    {
        return index switch
        {
            0 => OAuthScopes.ReadActivities,
            1 => OAuthScopes.WriteActivities,
            2 => OAuthScopes.ReadApplicationBuilds,
            3 => OAuthScopes.UploadApplicationBuilds,
            4 => OAuthScopes.ApplicationCommands,
            5 => OAuthScopes.UpdateApplicationCommands,
            6 => OAuthScopes.UpdateApplicationCommandPermissions,
            7 => OAuthScopes.ApplicationEntitlements,
            8 => OAuthScopes.UpdateApplicationStore,
            9 => OAuthScopes.Bot,
            10 => OAuthScopes.Connections,
            11 => OAuthScopes.ReadDMChannels,
            12 => OAuthScopes.Email,
            13 => OAuthScopes.JoinGroupDM,
            14 => OAuthScopes.Guilds,
            15 => OAuthScopes.JoinGuilds,
            16 => OAuthScopes.ReadGuildMembers,
            17 => OAuthScopes.Identify,
            18 => OAuthScopes.ReadMessages,
            19 => OAuthScopes.ReadRelationships,
            20 => OAuthScopes.WriteRoleConnections,
            21 => OAuthScopes.Rpc,
            22 => OAuthScopes.WriteRpcActivities,
            23 => OAuthScopes.ReadRpcNotifications,
            24 => OAuthScopes.ReadRpcVoice,
            25 => OAuthScopes.WriteRpcVoice,
            26 => OAuthScopes.Voice,
            27 => OAuthScopes.IncomingWebhook,
            _ => null
        };
    }
}
