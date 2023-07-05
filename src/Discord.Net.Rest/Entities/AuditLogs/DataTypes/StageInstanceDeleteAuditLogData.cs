using System.Linq;
using System.Text.Json;
using EntryModel = Discord.API.AuditLogEntry;
using Model = Discord.API.AuditLog;

namespace Discord.Rest;

/// <summary>
///     Contains a piece of audit log data related to a stage instance deleted.
/// </summary>
public partial class StageInstanceDeleteAuditLogData : IAuditLogData
{
    /// <summary>
    ///     Gets the topic of the stage channel.
    /// </summary>
    public string Topic { get; }

    /// <summary>
    ///     Gets the privacy level of the stage channel.
    /// </summary>
    public StagePrivacyLevel PrivacyLevel { get; }

    /// <summary>
    ///     Gets the user who started the stage channel.
    /// </summary>
    public IUser User { get; }

    /// <summary>
    ///     Gets the Id of the stage channel.
    /// </summary>
    public ulong StageChannelId { get; }

    internal StageInstanceDeleteAuditLogData(string topic, StagePrivacyLevel privacyLevel, IUser user, ulong channelId)
    {
        Topic = topic;
        PrivacyLevel = privacyLevel;
        User = user;
        StageChannelId = channelId;
    }

    internal static StageInstanceDeleteAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log)
    {
        var topic = entry.Changes!.FirstOrDefault(x => x.ChangedProperty == "topic")!.OldValue.Deserialize<string>(discord.ApiClient.SerializerOptions)!;
        var privacyLevel = entry.Changes!.FirstOrDefault(x => x.ChangedProperty == "privacy_level")!.OldValue.Deserialize<StagePrivacyLevel>(discord.ApiClient.SerializerOptions);
        var user = log?.Users?.FirstOrDefault(x => x.Id == entry.UserId)!;
        var channelId = entry.Options!.ChannelId;

        return new StageInstanceDeleteAuditLogData(topic, privacyLevel, RestUser.Create(discord, user), channelId ?? 0);
    }
}
