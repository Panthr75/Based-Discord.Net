using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest;

/// <summary>
///     Contains a piece of audit log data related to a voice channel status delete.
/// </summary>
public partial class VoiceChannelStatusDeletedAuditLogData : IAuditLogData
{
    private VoiceChannelStatusDeletedAuditLogData(ulong channelId)
    {
        ChannelId = channelId;
    }

    internal static VoiceChannelStatusDeletedAuditLogData Create(EntryModel entry)
    {
        return new(entry.TargetId!.Value);
    }

    /// <summary>
    ///     Get the id of the channel status was removed in.
    /// </summary>
    public ulong ChannelId { get; }
}
