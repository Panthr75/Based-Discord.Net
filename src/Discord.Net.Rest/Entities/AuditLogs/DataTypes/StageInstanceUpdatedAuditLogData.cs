using System.Linq;
using System.Text.Json;
using EntryModel = Discord.API.AuditLogEntry;
using Model = Discord.API.AuditLog;

namespace Discord.Rest;

/// <summary>
///     Contains a piece of audit log data related to a stage instance update.
/// </summary>
public partial class StageInstanceUpdatedAuditLogData : IAuditLogData
{
    /// <summary>
    ///     Gets the Id of the stage channel.
    /// </summary>
    public ulong StageChannelId { get; }

    /// <summary>
    ///     Gets the stage information before the changes.
    /// </summary>
    public StageInfo Before { get; }

    /// <summary>
    ///     Gets the stage information after the changes.
    /// </summary>
    public StageInfo After { get; }

    internal StageInstanceUpdatedAuditLogData(ulong channelId, StageInfo before, StageInfo after)
    {
        StageChannelId = channelId;
        Before = before;
        After = after;
    }

    internal static StageInstanceUpdatedAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log)
    {
        var channelId = entry.Options!.ChannelId!.Value;

        var topic = entry.Changes!.FirstOrDefault(x => x.ChangedProperty == "topic")!;
        var privacy = entry.Changes!.FirstOrDefault(x => x.ChangedProperty == "privacy")!;

        var user = RestUser.Create(discord, log?.Users?.FirstOrDefault(x => x.Id == entry.UserId)!);

        var oldTopic = topic?.OldValue.Deserialize<string>(discord.ApiClient.SerializerOptions)!;
        var newTopic = topic?.NewValue.Deserialize<string>(discord.ApiClient.SerializerOptions)!;
        var oldPrivacy = privacy?.OldValue.Deserialize<StagePrivacyLevel>(discord.ApiClient.SerializerOptions);
        var newPrivacy = privacy?.NewValue.Deserialize<StagePrivacyLevel>(discord.ApiClient.SerializerOptions);

        return new StageInstanceUpdatedAuditLogData(channelId, new StageInfo(user, oldPrivacy, oldTopic), new StageInfo(user, newPrivacy, newTopic));
    }
}
