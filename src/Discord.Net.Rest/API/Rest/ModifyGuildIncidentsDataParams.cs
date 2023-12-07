using System;
using System.Text.Json.Serialization;

namespace Discord.API.Rest;

internal class ModifyGuildIncidentsDataParams
{
    [JsonPropertyName("invites_disabled_until")]
    public Optional<DateTimeOffset?> InvitesDisabledUntil { get; set; }

    [JsonPropertyName("dms_disabled_until")]
    public Optional<DateTimeOffset?> DmsDisabledUntil { get; set; }
}
