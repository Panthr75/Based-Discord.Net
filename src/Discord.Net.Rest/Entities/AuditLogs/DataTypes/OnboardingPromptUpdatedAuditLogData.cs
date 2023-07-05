using Discord.API.AuditLogs;

using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest;


/// <summary>
///     Contains a piece of audit log data related to an onboarding prompt update.
/// </summary>
public partial class OnboardingPromptUpdatedAuditLogData : IAuditLogData
{
    internal OnboardingPromptUpdatedAuditLogData(OnboardingPromptInfo before, OnboardingPromptInfo after)
    {
        Before = before;
        After = after;
    }

    internal static OnboardingPromptUpdatedAuditLogData Create(BaseDiscordClient discord, EntryModel entry)
    {
        var changes = entry.Changes!;

        var (before, after) = AuditLogHelper.CreateAuditLogEntityInfo<OnboardingPromptAuditLogModel>(changes, discord);

        return new OnboardingPromptUpdatedAuditLogData(new(before, discord), new(after, discord));
    }

    /// <summary>
    ///     Gets the onboarding prompt information after the changes.
    /// </summary>
    public OnboardingPromptInfo After { get; set; }

    /// <summary>
    ///     Gets the onboarding prompt information before the changes.
    /// </summary>
    public OnboardingPromptInfo Before { get; set; }
}
