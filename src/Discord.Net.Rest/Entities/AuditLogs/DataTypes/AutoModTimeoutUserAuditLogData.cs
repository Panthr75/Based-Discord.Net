using EntryModel = Discord.API.AuditLogEntry;
using Model = Discord.API.AuditLog;

namespace Discord.Rest;

/// <summary>
///     Contains a piece of audit log data related to user getting in timeout by automod.
/// </summary>
public partial class AutoModTimeoutUserAuditLogData : IAuditLogData
{
    internal AutoModTimeoutUserAuditLogData(ulong channelId, string autoModRuleName, AutoModTriggerType autoModRuleTriggerType)
    {
        ChannelId = channelId;
        AutoModRuleName = autoModRuleName;
        AutoModRuleTriggerType = autoModRuleTriggerType;
    }

    internal static AutoModTimeoutUserAuditLogData Create(EntryModel entry)
    {
        return new(entry.Options!.ChannelId!.Value, entry.Options.AutoModRuleName!,
            entry.Options.AutoModRuleTriggerType!.Value);
    }

    /// <summary>
    ///     Gets the channel the message was sent in.
    /// </summary>
    public ulong ChannelId { get; set; }

    /// <summary>
    ///     Gets the name of the auto moderation rule that got triggered.
    /// </summary>
    public string AutoModRuleName { get; set; }

    /// <summary>
    ///     Gets the trigger type of the auto moderation rule that got triggered.
    /// </summary>
    public AutoModTriggerType AutoModRuleTriggerType { get; set; }
}
