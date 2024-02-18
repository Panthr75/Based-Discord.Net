using System.Runtime.CompilerServices;

namespace Discord.OAuth;

internal static class OAuthKnownScopes
{
    public const uint ReadActivities = 1u; // 1 << 0
    public const uint WriteActivities = 2u; // 1 << 1
    public const uint ReadApplicationBuilds = 4u; // 1 << 2
    public const uint UploadApplicationBuilds = 8u; // 1 << 3
    public const uint ApplicationCommands = 16u; // 1 << 4
    public const uint UpdateApplicationCommands = 32u; // 1 << 5
    public const uint UpdateApplicationCommandPermissions = 64u; // 1 << 6
    public const uint ApplicationEntitlements = 128u; // 1 << 7
    public const uint UpdateApplicationStore = 256u; // 1 << 8
    public const uint Bot = 512u; // 1 << 9
    public const uint Connections = 1024u; // 1 << 10
    public const uint ReadDMChannels = 2048u; // 1 << 11
    public const uint Email = 4096u; // 1 << 12
    public const uint JoinGroupDM = 8192u; // 1 << 13
    public const uint Guilds = 16384u; // 1 << 14
    public const uint JoinGuilds = 32768u; // 1 << 15
    public const uint ReadGuildMembers = 65536u; // 1 << 16
    public const uint Identify = 131072u; // 1 << 17
    public const uint ReadMessages = 262144u; // 1 << 18
    public const uint ReadRelationships = 524288u; // 1 << 19
    public const uint WriteRoleConnections = 1048576u; // 1 << 20
    public const uint Rpc = 2097152u; // 1 << 21
    public const uint WriteRpcActivities = 4194304u; // 1 << 22
    public const uint ReadRpcNotifications = 8388608u; // 1 << 23
    public const uint ReadRpcVoice = 16777216u; // 1 << 24
    public const uint WriteRpcVoice = 33554432u; // 1 << 25
    public const uint Voice = 67108864u; // 1 << 26
    public const uint IncomingWebhook = 134217728u; // 1 << 27

    public const int Count = 28; 

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint GetRawValue(bool readActivities = false,
        bool writeActivities = false,
        bool readApplicationBuilds = false,
        bool uploadApplicationBuilds = false,
        bool applicationCommands = false,
        bool updateApplicationCommands = false,
        bool updateApplicationCommandPermissions = false,
        bool applicationEntitlements = false,
        bool updateApplicationStore = false,
        bool bot = false,
        bool connections = false,
        bool readDMChannels = false,
        bool email = false,
        bool joinGroupDM = false,
        bool guilds = false,
        bool joinGuilds = false,
        bool readGuildMembers = false,
        bool identify = false,
        bool readMessages = false,
        bool readRelationships = false,
        bool writeRoleConnections = false,
        bool rpc = false,
        bool writeRpcActivities = false,
        bool readRpcNotifications = false,
        bool readRpcVoice = false,
        bool writeRpcVoice = false,
        bool voice = false,
        bool incomingWebhook = false)
    {
        return
            (readActivities ? OAuthKnownScopes.ReadActivities : 0U) |
            (writeActivities ? OAuthKnownScopes.WriteActivities : 0U) |
            (readApplicationBuilds ? OAuthKnownScopes.ReadApplicationBuilds : 0U) |
            (uploadApplicationBuilds ? OAuthKnownScopes.UploadApplicationBuilds : 0U) |
            (applicationCommands ? OAuthKnownScopes.ApplicationCommands : 0U) |
            (updateApplicationCommands ? OAuthKnownScopes.UpdateApplicationCommands : 0U) |
            (updateApplicationCommandPermissions ? OAuthKnownScopes.UpdateApplicationCommandPermissions : 0U) |
            (applicationEntitlements ? OAuthKnownScopes.ApplicationEntitlements : 0U) |
            (updateApplicationStore ? OAuthKnownScopes.UpdateApplicationStore : 0U) |
            (bot ? OAuthKnownScopes.Bot : 0U) |
            (connections ? OAuthKnownScopes.Connections : 0U) |
            (readDMChannels ? OAuthKnownScopes.ReadDMChannels : 0U) |
            (email ? OAuthKnownScopes.Email : 0U) |
            (joinGroupDM ? OAuthKnownScopes.JoinGroupDM : 0U) |
            (guilds ? OAuthKnownScopes.Guilds : 0U) |
            (joinGuilds ? OAuthKnownScopes.JoinGuilds : 0U) |
            (readGuildMembers ? OAuthKnownScopes.ReadGuildMembers : 0U) |
            (identify ? OAuthKnownScopes.Identify : 0U) |
            (readMessages ? OAuthKnownScopes.ReadMessages : 0U) |
            (readRelationships ? OAuthKnownScopes.ReadRelationships : 0U) |
            (writeRoleConnections ? OAuthKnownScopes.WriteRoleConnections : 0U) |
            (rpc ? OAuthKnownScopes.Rpc : 0U) |
            (writeRpcActivities ? OAuthKnownScopes.WriteRpcActivities : 0U) |
            (readRpcNotifications ? OAuthKnownScopes.ReadRpcNotifications : 0U) |
            (readRpcVoice ? OAuthKnownScopes.ReadRpcVoice : 0U) |
            (writeRpcVoice ? OAuthKnownScopes.WriteRpcVoice : 0U) |
            (voice ? OAuthKnownScopes.Voice : 0U) |
            (incomingWebhook ? OAuthKnownScopes.IncomingWebhook : 0U);
    }
}