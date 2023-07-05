using Discord.API.AuditLogs;
using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest;

/// <summary>
///     Contains a piece of audit log data related to a sticker creation.
/// </summary>
public partial class StickerCreatedAuditLogData : IAuditLogData
{
    internal StickerCreatedAuditLogData(StickerInfo data)
    {
        Data = data;
    }

    internal static StickerCreatedAuditLogData Create(BaseDiscordClient discord, EntryModel entry)
    {
        var changes = entry.Changes!;
        var (_, data) = AuditLogHelper.CreateAuditLogEntityInfo<StickerInfoAuditLogModel>(changes, discord);

        return new StickerCreatedAuditLogData(new(data));
    }

    /// <summary>
    ///     Gets the sticker information after the changes.
    /// </summary>
    public StickerInfo Data { get; }
}
