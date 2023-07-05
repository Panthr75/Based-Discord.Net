using System.Linq;
using System.Text.Json;
using EntryModel = Discord.API.AuditLogEntry;
using Model = Discord.API.AuditLog;

namespace Discord.Rest;

/// <summary>
///     Contains a piece of audit log data related to an emoji update.
/// </summary>
public partial class EmoteUpdateAuditLogData : IAuditLogData
{
    private EmoteUpdateAuditLogData(ulong id, string oldName, string newName)
    {
        EmoteId = id;
        OldName = oldName;
        NewName = newName;
    }

    internal static EmoteUpdateAuditLogData Create(BaseDiscordClient discord, EntryModel entry)
    {
        var change = entry.Changes!.FirstOrDefault(x => x.ChangedProperty == "name");

        var newName = change?.NewValue?.Deserialize<string>(discord.ApiClient.SerializerOptions);
        var oldName = change?.OldValue?.Deserialize<string>(discord.ApiClient.SerializerOptions);

        return new EmoteUpdateAuditLogData(entry.TargetId!.Value, oldName!, newName!);
    }

    /// <summary>
    ///     Gets the snowflake ID of the updated emoji.
    /// </summary>
    /// <returns>
    ///     A <see cref="ulong"/> representing the snowflake identifier of the updated emoji.
    /// </returns>
    public ulong EmoteId { get; }

    /// <summary>
    ///     Gets the new name of the updated emoji.
    /// </summary>
    /// <returns>
    ///     A string containing the new name of the updated emoji.
    /// </returns>
    public string NewName { get; }

    /// <summary>
    ///     Gets the old name of the updated emoji.
    /// </summary>
    /// <returns>
    ///     A string containing the old name of the updated emoji.
    /// </returns>
    public string OldName { get; }
}
