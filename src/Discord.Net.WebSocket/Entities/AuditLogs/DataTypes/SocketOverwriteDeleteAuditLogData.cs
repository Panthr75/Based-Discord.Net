using System.Linq;
using System.Text.Json;
using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.WebSocket;

/// <summary>
///     Contains a piece of audit log data related to the deletion of a permission overwrite.
/// </summary>
public class SocketOverwriteDeleteAuditLogData : ISocketAuditLogData
{
    private SocketOverwriteDeleteAuditLogData(ulong channelId, Overwrite deletedOverwrite)
    {
        ChannelId = channelId;
        Overwrite = deletedOverwrite;
    }

    internal static SocketOverwriteDeleteAuditLogData Create(DiscordSocketClient discord, EntryModel entry)
    {
        var changes = entry.Changes!;

        var denyModel = changes.FirstOrDefault(x => x.ChangedProperty == "deny")!;
        var allowModel = changes.FirstOrDefault(x => x.ChangedProperty == "allow")!;

        var deny = denyModel.OldValue.Deserialize<ulong>(discord.ApiClient.SerializerOptions);
        var allow = allowModel.OldValue.Deserialize<ulong>(discord.ApiClient.SerializerOptions);

        var permissions = new OverwritePermissions(allow, deny);

        var id = entry.Options!.OverwriteTargetId!.Value;
        var type = entry.Options!.OverwriteType;

        return new SocketOverwriteDeleteAuditLogData(entry.TargetId!.Value, new Overwrite(id, type, permissions));
    }

    /// <summary>
    ///     Gets the ID of the channel that the overwrite was deleted from.
    /// </summary>
    /// <returns>
    ///     A <see cref="ulong"/> representing the snowflake identifier for the channel that the overwrite was
    ///     deleted from.
    /// </returns>
    public ulong ChannelId { get; }

    /// <summary>
    ///     Gets the permission overwrite object that was deleted.
    /// </summary>
    /// <returns>
    ///     An <see cref="Overwrite"/> object representing the overwrite that was deleted.
    /// </returns>
    public Overwrite Overwrite { get; }
}
