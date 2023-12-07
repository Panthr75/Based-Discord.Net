using System;
using System.Text.Json.Serialization;

namespace Discord.API;

internal class GuildIncidentsData
{
    [JsonPropertyName("invites_disabled_until")]
    public DateTimeOffset? InvitesDisabledUntil { get; set; }

    [JsonPropertyName("dms_disabled_until")]
    public DateTimeOffset? DmsDisabledUntil { get; set; }
}
