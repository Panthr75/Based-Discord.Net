using Discord.API.AuditLogs;

using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest;


/// <summary>
///     Contains a piece of audit log data related to a guild update.
/// </summary>
public partial class OnboardingUpdatedAuditLogData : IAuditLogData
{
    internal OnboardingUpdatedAuditLogData(OnboardingInfo before, OnboardingInfo after)
    {
        Before = before;
        After = after;
    }

    internal static OnboardingUpdatedAuditLogData Create(BaseDiscordClient discord, EntryModel entry)
    {
        var changes = entry.Changes!;

        var (before, after) = AuditLogHelper.CreateAuditLogEntityInfo<OnboardingAuditLogModel>(changes, discord);

        return new OnboardingUpdatedAuditLogData(new(before, discord), new(after, discord));
    }

    /// <summary>
    ///     Gets the onboarding information after the changes.
    /// </summary>
    public OnboardingInfo After { get; set; }

    /// <summary>
    ///     Gets the onboarding information before the changes.
    /// </summary>
    public OnboardingInfo Before { get; set; }
}
