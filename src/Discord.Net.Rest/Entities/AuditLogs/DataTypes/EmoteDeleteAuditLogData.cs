using System.Linq;
using System.Text.Json;
using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest;

/// <summary>
///     Contains a piece of audit log data related to an emoji deletion.
/// </summary>
public partial class EmoteDeleteAuditLogData : IAuditLogData
{
    private EmoteDeleteAuditLogData(ulong id, string name)
    {
        EmoteId = id;
        Name = name;
    }

    internal static EmoteDeleteAuditLogData Create(BaseDiscordClient discord, EntryModel entry)
    {
        var change = entry.Changes!.FirstOrDefault(x => x.ChangedProperty == "name");

        var emoteName = change?.OldValue?.Deserialize<string>(discord.ApiClient.SerializerOptions);

        return new EmoteDeleteAuditLogData(entry.TargetId!.Value, emoteName!);
    }

    /// <summary>
    ///     Gets the snowflake ID of the deleted emoji.
    /// </summary>
    /// <returns>
    ///     A <see cref="ulong"/> representing the snowflake identifier for the deleted emoji.
    /// </returns>
    public ulong EmoteId { get; }

    /// <summary>
    ///     Gets the name of the deleted emoji.
    /// </summary>
    /// <returns>
    ///     A string containing the name of the deleted emoji.
    /// </returns>
    public string Name { get; }
}
