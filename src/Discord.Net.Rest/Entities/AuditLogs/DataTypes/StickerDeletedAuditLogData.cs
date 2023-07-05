using Discord.API.AuditLogs;
using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest;

/// <summary>
///     Contains a piece of audit log data related to a sticker removal.
/// </summary>
public partial class StickerDeletedAuditLogData : IAuditLogData
{
    internal StickerDeletedAuditLogData(StickerInfo data)
    {
        Data = data;
    }

    internal static StickerDeletedAuditLogData Create(BaseDiscordClient discord, EntryModel entry)
    {
        var changes = entry.Changes!;
        var (data, _) = AuditLogHelper.CreateAuditLogEntityInfo<StickerInfoAuditLogModel>(changes, discord);

        return new StickerDeletedAuditLogData(new(data));
    }

    /// <summary>
    ///     Gets the sticker information before the changes.
    /// </summary>
    public StickerInfo Data { get; }
}
