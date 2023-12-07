using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest;

/// <summary>
///     Contains a piece of audit log data related to a voice channel status update.
/// </summary>
public partial class VoiceChannelStatusUpdateAuditLogData : IAuditLogData
{
    private VoiceChannelStatusUpdateAuditLogData(string? status, ulong channelId)
    {
        Status = status;
        ChannelId = channelId;
    }

    internal static VoiceChannelStatusUpdateAuditLogData Create(EntryModel entry)
    {
        return new(entry.Options!.Status, entry.TargetId!.Value);
    }

    /// <summary>
    ///     Gets the status that was set in the voice channel.
    /// </summary>
    public string? Status { get; }

    /// <summary>
    ///     Get the id of the channel status was updated in.
    /// </summary>
    public ulong ChannelId { get; }
}
