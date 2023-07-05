using Discord.API.AuditLogs;

using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest;

/// <summary>
///     Contains a piece of audit log data related to an onboarding prompt creation.
/// </summary>
public partial class OnboardingPromptCreatedAuditLogData : IAuditLogData
{
    internal OnboardingPromptCreatedAuditLogData(OnboardingPromptInfo data)
    {
        Data = data;
    }

    internal static OnboardingPromptCreatedAuditLogData Create(BaseDiscordClient discord, EntryModel entry)
    {
        var changes = entry.Changes!;

        var (_, data) = AuditLogHelper.CreateAuditLogEntityInfo<OnboardingPromptAuditLogModel>(changes, discord);

        return new OnboardingPromptCreatedAuditLogData(new(data, discord));
    }

    /// <summary>
    ///     Gets the onboarding prompt information after the changes.
    /// </summary>
    public OnboardingPromptInfo Data { get; set; }
}
