using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest;

/// <summary>
///     Contains a piece of audit log data related to disconnecting members from voice channels.
/// </summary>
public partial class MemberDisconnectAuditLogData : IAuditLogData
{
    private MemberDisconnectAuditLogData(int count)
    {
        MemberCount = count;
    }

    internal static MemberDisconnectAuditLogData Create(EntryModel entry)
    {
        return new MemberDisconnectAuditLogData(entry.Options!.Count!.Value);
    }

    /// <summary>
    ///     Gets the number of members that were disconnected.
    /// </summary>
    /// <returns>
    ///     An <see cref="int"/> representing the number of members that were disconnected from a voice channel.
    /// </returns>
    public int MemberCount { get; }
}
